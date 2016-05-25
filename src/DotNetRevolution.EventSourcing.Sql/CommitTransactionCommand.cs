using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Collections.Generic;
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

        public CommitTransactionCommand(SqlSerializer serializer,
            GetSnapshotIfPolicySatisfiedDelegate getSnapshotIfPolicySatisfiedDelegate,            
            Transaction transaction)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(getSnapshotIfPolicySatisfiedDelegate != null);
            Contract.Requires(transaction != null);

            _serializer = serializer;
            _getSnapshotIfPolicySatisfiedDelegate = getSnapshotIfPolicySatisfiedDelegate;

            _command = CreateSqlCommand(transaction.Identity, transaction.Command, transaction.EventProviders, transaction.User);            
        }

        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            // execute command
            _command.ExecuteNonQuery();
        }
        
        private SqlCommand CreateSqlCommand(Identity transactionId, ICommand command, IReadOnlyCollection<EventProvider> eventProviders, string user)
        {
            var sqlCommand = new SqlCommand("[dbo].[CreateTransaction]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var commandType = new TransactionCommandType(command.GetType());

            // set parameters
            sqlCommand.Parameters.Add("@transactionGuid", SqlDbType.UniqueIdentifier).Value = transactionId.Value;
            sqlCommand.Parameters.Add("@user", SqlDbType.NVarChar, 256).Value = user;
            sqlCommand.Parameters.Add("@commandGuid", SqlDbType.UniqueIdentifier).Value = command.CommandId;
            sqlCommand.Parameters.Add("@commandTypeFullName", SqlDbType.VarChar, 512).Value = commandType.FullName;
            sqlCommand.Parameters.Add("@commandData", SqlDbType.VarBinary).Value = _serializer.SerializeObject(command);
            
            DataTable eventProviderDataTable;
            DataTable eventDataTable;
            DataTable snapshotDataTable;

            GetDataTables(eventProviders, out eventProviderDataTable, out eventDataTable, out snapshotDataTable);

            // event provider user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@eventProviders", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEventProvider]",
                Value = eventProviderDataTable
            });

            // event user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@events", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEvent]",
                Value = eventDataTable
            });

            // snapshot user defined table type
            sqlCommand.Parameters.Add(new SqlParameter("@eventProviderSnapshots", SqlDbType.Structured)
            {
                TypeName = "[dbo].[udttEventProviderSnapshot]",
                Value = snapshotDataTable
            });

            return sqlCommand;
        }

        private void GetDataTables(IReadOnlyCollection<EventProvider> eventProviders, out DataTable eventProviderDataTable, out DataTable eventDataTable, out DataTable snapshotDataTable)
        {
            // create data tables
            eventProviderDataTable = CreateEventProviderDataTable();
            eventDataTable = CreateEventDataTable();
            snapshotDataTable = CreateSnapshotDataTable();

            // variable to create temporary ids
            var eventProviderTempId = -1;

            // go through each data provider
            foreach (var eventProvider in eventProviders)
            {
                AddEventProviderRow(eventProviderDataTable, ++eventProviderTempId, eventProvider);

                // add snapshot
                AddSnapshot(snapshotDataTable, eventProviderTempId, eventProvider);

                // new sequence object to keep track of the sequence per event provider
                var sequence = new TransactionEventSequence();

                // go through each domain event in the event provider
                foreach (var domainEvent in eventProvider.DomainEvents)
                {
                    AddEventRow(eventDataTable, eventProviderTempId, sequence.Increment(), domainEvent);
                }
            }
        }

        private void AddSnapshot(DataTable snapshotDataTable, int eventProviderTempId, EventProvider eventProvider)
        {
            // get snapshot if policy satisfied
            var snapshot = _getSnapshotIfPolicySatisfiedDelegate(eventProvider);

            // add snapshot was returned
            if (snapshot != null)
            {
                AddSnapshotRow(snapshotDataTable, eventProviderTempId, snapshot);
            }
        }

        private void AddEventRow(DataTable dataTable, int eventProviderTempId, int sequence, IDomainEvent domainEvent)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            var eventType = new TransactionEventType(domainEvent.GetType());

            // populate data row
            dataRow["EventProviderTempId"] = eventProviderTempId;
            dataRow["EventGuid"] = domainEvent.DomainEventId;
            dataRow["Sequence"] = sequence;
            dataRow["TypeFullName"] = eventType.FullName;
            dataRow["Data"] = _serializer.SerializeObject(domainEvent);

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }

        private static void AddEventProviderRow(DataTable dataTable, int eventProviderTempId, EventProvider eventProvider)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            // populate data row
            dataRow["TempId"] = eventProviderTempId;
            dataRow["EventProviderGuid"] = eventProvider.Identity.Value;
            dataRow["Descriptor"] = eventProvider.Descriptor.Value;
            dataRow["TypeFullName"] = eventProvider.EventProviderType.FullName;
            dataRow["Version"] = eventProvider.Version.Value;

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }

        private void AddSnapshotRow(DataTable dataTable, int eventProviderTempId, Snapshot snapshot)
        {
            // add row to the data table
            var dataRow = dataTable.NewRow();

            // populate data row            
            dataRow["EventProviderTempId"] = eventProviderTempId;
            dataRow["TypeFullName"] = snapshot.SnapshotType.FullName;
            dataRow["Data"] = _serializer.SerializeObject(snapshot.Data);

            // add new row to table
            dataTable.Rows.Add(dataRow);
        }

        private DataTable CreateEventDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("EventProviderTempId", typeof(int));
            dataTable.Columns.Add("EventGuid", typeof(Guid));
            dataTable.Columns.Add("Sequence", typeof(int));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }

        private DataTable CreateEventProviderDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("TempId", typeof(int));
            dataTable.Columns.Add("EventProviderGuid", typeof(Guid));
            dataTable.Columns.Add("Descriptor", typeof(string));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Version", typeof(int));

            return dataTable;
        }

        private DataTable CreateSnapshotDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("EventProviderTempId", typeof(int));
            dataTable.Columns.Add("TypeFullName", typeof(string));
            dataTable.Columns.Add("Data", typeof(byte[]));

            return dataTable;
        }
    }
}
