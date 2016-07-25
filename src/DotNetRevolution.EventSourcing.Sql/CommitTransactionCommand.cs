using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
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

        public CommitTransactionCommand(SqlSerializer serializer,
            GetSnapshotIfPolicySatisfiedDelegate getSnapshotIfPolicySatisfiedDelegate, 
            ITypeFactory typeFactory,           
            Transaction transaction)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(getSnapshotIfPolicySatisfiedDelegate != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(transaction != null);

            _serializer = serializer;
            _typeFactory = typeFactory;
            _getSnapshotIfPolicySatisfiedDelegate = getSnapshotIfPolicySatisfiedDelegate;

            _command = CreateSqlCommand(transaction.Identity, transaction.Command, transaction.EventProvider, transaction.User);            
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
            
            sqlCommand.Parameters.Add("@eventProviderGuid", SqlDbType.UniqueIdentifier).Value = SequentialGuid.Create();
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = eventProvider.Identity.Value;
            sqlCommand.Parameters.Add("@eventProviderTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(eventProvider.EventProviderType.Type);
            sqlCommand.Parameters.Add("@eventProviderTypeFullName", SqlDbType.VarChar, 512).Value = eventProvider.EventProviderType.Type.FullName;
            sqlCommand.Parameters.Add("@eventProviderDescriptor", SqlDbType.VarChar).Value = eventProvider.Descriptor.Value;
            sqlCommand.Parameters.Add("@eventProviderVersion", SqlDbType.Int).Value = eventProvider.Version.Value;
            
            DataTable eventDataTable;
            DataTable snapshotDataTable;

            GetDataTables(eventProvider, out eventDataTable, out snapshotDataTable);
            
            // event user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@events", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEvent]",
                Value = eventDataTable
            });

            // snapshot user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@eventProviderSnapshot", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEventProviderSnapshot]",
                Value = snapshotDataTable
            });

            return sqlCommand;
        }

        private void GetDataTables(EventProvider eventProvider, out DataTable eventDataTable, out DataTable snapshotDataTable)
        {
            // create data tables            
            eventDataTable = CreateEventDataTable();
            snapshotDataTable = CreateSnapshotDataTable();
                        
            // add snapshot
            AddSnapshot(snapshotDataTable, eventProvider);

            // new sequence object to keep track of the sequence per event provider
            var sequence = new TransactionEventSequence();

            // go through each domain event in the event provider
            foreach (var domainEvent in eventProvider.DomainEvents)
            {
                AddEventRow(eventDataTable, eventProvider, sequence.Increment(), domainEvent);
            }
        }

        private void AddSnapshot(DataTable snapshotDataTable, EventProvider eventProvider)
        {
            // get snapshot if policy satisfied
            var snapshot = _getSnapshotIfPolicySatisfiedDelegate(eventProvider);

            // add snapshot was returned
            if (snapshot != null)
            {
                AddSnapshotRow(snapshotDataTable, eventProvider, snapshot);
            }
        }

        private void AddEventRow(DataTable dataTable, EventProvider eventProvider, int sequence, IDomainEvent domainEvent)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            var eventType = new TransactionEventType(domainEvent.GetType());

            // populate data row
            dataRow["EventProviderGuid"] = eventProvider.Identity.Value;
            dataRow["EventId"] = domainEvent.DomainEventId;
            dataRow["Sequence"] = sequence;
            dataRow["TypeId"] = _typeFactory.GetHash(domainEvent.GetType());
            dataRow["TypeFullName"] = eventType.FullName;
            dataRow["Data"] = _serializer.SerializeObject(domainEvent);

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }
                
        private void AddSnapshotRow(DataTable dataTable, EventProvider eventProvider, Snapshot snapshot)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            // populate data row            
            dataRow["TypeId"] = _typeFactory.GetHash(snapshot.SnapshotType.Type);
            dataRow["TypeFullName"] = snapshot.SnapshotType.Type.FullName;
            dataRow["Data"] = _serializer.SerializeObject(snapshot.Data);

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }

        private DataTable CreateEventDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("EventProviderGuid", typeof(Guid));
            dataTable.Columns.Add("EventId", typeof(Guid));
            dataTable.Columns.Add("Sequence", typeof(int));
            dataTable.Columns.Add("TypeId", typeof(byte[]));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }
        
        private DataTable CreateSnapshotDataTable()
        {
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("TypeId", typeof(byte[]));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }
    }
}
