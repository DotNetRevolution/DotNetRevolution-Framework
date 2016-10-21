using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class CommitTransactionCommand
    {        
        private const int EventProviderConcurrencyErrorNumber = 51000;

        private readonly SqlSerializer _serializer;
        private readonly SqlCommand _command;
        private readonly ITypeFactory _typeFactory;
        
        private readonly SqlParameter _result;

        public CommitTransactionCommand(SqlSerializer serializer,
            ITypeFactory typeFactory, 
            string username,          
            EventProviderTransaction transaction,
            IReadOnlyCollection<EventStreamRevision> uncommittedRevisions)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(transaction != null);
            Contract.Requires(uncommittedRevisions != null);
            Contract.Requires(string.IsNullOrWhiteSpace(username) == false);

            _serializer = serializer;
            _typeFactory = typeFactory;

            _result = new SqlParameter("result", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };

            _command = CreateSqlCommand(transaction, uncommittedRevisions, username);
        }

        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            try
            {
                // execute command
                _command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                ThrowIfEventProviderConcurrencyError(e);
            }

            ThrowIfError();
        }

        public async Task ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            try
            {
                // execute command
                await _command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                ThrowIfEventProviderConcurrencyError(e);
            }

            ThrowIfError();
        }

        private static void ThrowIfEventProviderConcurrencyError(SqlException e)
        {
            if (e.Number == EventProviderConcurrencyErrorNumber)
            {
                throw new AggregateRootConcurrencyException();
            }
        }

        private void ThrowIfError()
        {
            if ((int)_result.Value != 0)
            {
                throw new ApplicationException("A problem occurred while processing transaction.");
            }
        }

        private SqlCommand CreateSqlCommand(EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions, string user)
        {
            var sqlCommand = new SqlCommand("[dbo].[CreateTransaction]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var command = transaction.Command;
            var eventProvider = transaction.EventStream.EventProvider;
            var commandType = command.GetType();

            // set return parameter
            sqlCommand.Parameters.Add(_result);

            // set parameters
            sqlCommand.Parameters.Add("@transactionId", SqlDbType.UniqueIdentifier).Value = transaction.Identity.Value;
            sqlCommand.Parameters.Add("@user", SqlDbType.NVarChar, 256).Value = user;

            sqlCommand.Parameters.Add("@commandId", SqlDbType.UniqueIdentifier).Value = command.CommandId;
            sqlCommand.Parameters.Add("@commandTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(commandType);
            sqlCommand.Parameters.Add("@commandTypeFullName", SqlDbType.VarChar, 512).Value = commandType.FullName;
            sqlCommand.Parameters.Add("@commandData", SqlDbType.VarBinary).Value = _serializer.SerializeObject(command);
            
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = eventProvider.EventProviderIdentity.Value;
            sqlCommand.Parameters.Add("@aggregateRootId", SqlDbType.UniqueIdentifier).Value = eventProvider.AggregateRootIdentity.Value;
            sqlCommand.Parameters.Add("@aggregateRootTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(eventProvider.AggregateRootType.Type);
            sqlCommand.Parameters.Add("@aggregateRootTypeFullName", SqlDbType.VarChar, 512).Value = eventProvider.AggregateRootType.Type.FullName;
            sqlCommand.Parameters.Add("@eventProviderDescriptor", SqlDbType.VarChar).Value = transaction.Descriptor.Value;
            
            // event user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@events", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEvent]",
                Value = GetEventDataTable(uncommittedRevisions)
            });            

            return sqlCommand;
        }
        
        private DataTable GetEventDataTable(IEnumerable<EventStreamRevision> eventStream)
        {
            // create data tables            
            var eventDataTable = CreateEventDataTable();
                        
            // new sequence object to keep track of the sequence per event provider
            var sequence = new TransactionEventSequence();

            // go through each domain event in the event provider
            foreach (var revision in eventStream.Where(x => x is DomainEventRevision).Cast<DomainEventRevision>())
            {
                foreach(var domainEvent in revision.DomainEvents)
                {
                    AddEventRow(eventDataTable, sequence.Increment(), revision.Version, domainEvent);
                }
            }

            return eventDataTable;
        }
        
        private void AddEventRow(DataTable dataTable, int sequence, int version, IDomainEvent domainEvent)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            var eventType = new TransactionEventType(domainEvent.GetType());

            // populate data row
            dataRow["EventId"] = domainEvent.DomainEventId;
            dataRow["EventProviderVersion"] = version;
            dataRow["Sequence"] = sequence;
            dataRow["TypeId"] = _typeFactory.GetHash(domainEvent.GetType());
            dataRow["TypeFullName"] = eventType.FullName;
            dataRow["Data"] = _serializer.SerializeObject(domainEvent);

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }                

        private DataTable CreateEventDataTable()
        {
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("EventId", typeof(Guid));
            dataTable.Columns.Add("EventProviderVersion", typeof(int));
            dataTable.Columns.Add("Sequence", typeof(int));
            dataTable.Columns.Add("TypeId", typeof(byte[]));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }        
    }
}
