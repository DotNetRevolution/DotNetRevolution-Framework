using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class CommitTransactionCommand
    {
        private readonly SqlSerializer _serializer;
        private readonly SqlCommand _command;
        private readonly GetSnapshotIfPolicySatisfiedDelegate _getSnapshotIfPolicySatisfiedDelegate;
        private readonly ITypeFactory _typeFactory;
        private readonly IGuidGenerator _guidGenerator;

        public CommitTransactionCommand(SqlSerializer serializer,
            GetSnapshotIfPolicySatisfiedDelegate getSnapshotIfPolicySatisfiedDelegate, 
            ITypeFactory typeFactory, 
            string username,          
            Transaction transaction,
            IGuidGenerator guidGenerator)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(getSnapshotIfPolicySatisfiedDelegate != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(transaction != null);
            Contract.Requires(guidGenerator != null);
            Contract.Requires(string.IsNullOrWhiteSpace(username) == false);

            _serializer = serializer;
            _typeFactory = typeFactory;
            _guidGenerator = guidGenerator;
            _getSnapshotIfPolicySatisfiedDelegate = getSnapshotIfPolicySatisfiedDelegate;

            _command = CreateSqlCommand(transaction.Identity, transaction.Command, transaction.EventProvider, username);            
        }

        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            // execute command
            _command.ExecuteNonQuery();
        }
        
        private SqlCommand CreateSqlCommand(Identity transactionId, ICommand command, EventProvider eventProvider, string user)
        {
            var sqlCommand = new SqlCommand("[dbo].[CreateTransaction]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var commandType = command.GetType();

            // set parameters
            sqlCommand.Parameters.Add("@transactionId", SqlDbType.UniqueIdentifier).Value = transactionId.Value;
            sqlCommand.Parameters.Add("@user", SqlDbType.NVarChar, 256).Value = user;

            sqlCommand.Parameters.Add("@commandId", SqlDbType.UniqueIdentifier).Value = command.CommandId;
            sqlCommand.Parameters.Add("@commandTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(commandType);
            sqlCommand.Parameters.Add("@commandTypeFullName", SqlDbType.VarChar, 512).Value = commandType.FullName;
            sqlCommand.Parameters.Add("@commandData", SqlDbType.VarBinary).Value = _serializer.SerializeObject(command);
            
            sqlCommand.Parameters.Add("@eventProviderGuid", SqlDbType.UniqueIdentifier).Value = _guidGenerator.Create();
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = eventProvider.Identity.Value;
            sqlCommand.Parameters.Add("@eventProviderTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(eventProvider.EventProviderType.Type);
            sqlCommand.Parameters.Add("@eventProviderTypeFullName", SqlDbType.VarChar, 512).Value = eventProvider.EventProviderType.Type.FullName;
            sqlCommand.Parameters.Add("@eventProviderDescriptor", SqlDbType.VarChar).Value = eventProvider.Descriptor.Value;
            sqlCommand.Parameters.Add("@eventProviderVersion", SqlDbType.Int).Value = eventProvider.Version.Value;
            
            // event user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@events", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEvent]",
                Value = GetEventDataTable(eventProvider)
            });

            // snapshot
            AddSnapshotParameters(sqlCommand, eventProvider);

            return sqlCommand;
        }

        private void AddSnapshotParameters(SqlCommand sqlCommand, EventProvider eventProvider)
        {
            // get snapshot if policy satisfied
            var snapshot = _getSnapshotIfPolicySatisfiedDelegate(eventProvider);

            // add snapshot was returned
            if (snapshot != null)
            {
                sqlCommand.Parameters.Add("@snapshotTypeId", SqlDbType.Binary).Value = _typeFactory.GetHash(snapshot.SnapshotType.Type);
                sqlCommand.Parameters.Add("@snapshotTypeFullName", SqlDbType.VarChar).Value = snapshot.SnapshotType.Type.FullName;
                sqlCommand.Parameters.Add("@snapshotData", SqlDbType.VarBinary).Value = _serializer.SerializeObject(snapshot.Data);
            }
        }

        private DataTable GetEventDataTable(EventProvider eventProvider)
        {
            // create data tables            
            var eventDataTable = CreateEventDataTable();
                        
            // new sequence object to keep track of the sequence per event provider
            var sequence = new TransactionEventSequence();

            // go through each domain event in the event provider
            foreach (var domainEvent in eventProvider.DomainEvents)
            {
                AddEventRow(eventDataTable, sequence.Increment(), domainEvent);
            }

            return eventDataTable;
        }
        
        private void AddEventRow(DataTable dataTable, int sequence, IDomainEvent domainEvent)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            var eventType = new TransactionEventType(domainEvent.GetType());

            // populate data row
            dataRow["EventId"] = domainEvent.DomainEventId;
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
            dataTable.Columns.Add("Sequence", typeof(int));
            dataTable.Columns.Add("TypeId", typeof(byte[]));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }        
    }
}
