using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Text;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.Core.Helper;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private static readonly Encoding _encoding = Encoding.UTF8;

        private readonly ISerializer _serializer;
        private readonly string _connectionString;

        public SqlEventStore(IAggregateRootProcessorFactory eventStreamProcessorProvider,
                             ISnapshotPolicyFactory snapshotPolicyProvider,
                             ISnapshotProviderFactory snapshotProviderFactory,
                             ISerializer serializer,
                             string connectionString)
            : base(eventStreamProcessorProvider, snapshotPolicyProvider, snapshotProviderFactory, serializer)
        {
            Contract.Requires(eventStreamProcessorProvider != null);
            Contract.Requires(snapshotPolicyProvider != null);
            Contract.Requires(serializer != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = serializer;
        }

        protected override EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor, out Snapshot snapshot)
        {
            Collection<SqlDomainEvent> sqlDomainEvents;

            SqlSnapshot sqlSnapshot;

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // establish command
                SqlCommand command = CreateQueryCommand(conn, eventProviderType, identity);

                // connection needs to be open before executing
                conn.Open();

                // execute
                sqlDomainEvents = ExecuteQueryCommand(command, out eventProviderDescriptor, out sqlSnapshot, out version);
            }
                 
            // set snapshot       
            snapshot = sqlSnapshot == null 
                ? null
                : DeserializeSnapshot(sqlSnapshot);

            // return deserialized events
            if (sqlDomainEvents.Any() == false)
            {
                // return event stream with no events, snapshot only
                return new EventStream(new Collection<IDomainEvent>(), snapshot);
            }

            // return event stream with events and snapshot
            return new EventStream(DeserializeDomainEvents(sqlDomainEvents), snapshot);
        }

        private IReadOnlyCollection<IDomainEvent> DeserializeDomainEvents(Collection<SqlDomainEvent> sqlDomainEvents)
        {
            return new ReadOnlyCollection<IDomainEvent>(sqlDomainEvents
                .OrderBy(x => x.EventProviderVersion)
                .ThenBy(x => x.Sequence)
                .Select(x => DeserializeDomainEvent(x.EventType, x.Data))
                .ToList());
        }

        private static Collection<SqlDomainEvent> ExecuteQueryCommand(SqlCommand command, out EventProviderDescriptor eventProviderDescriptor, out SqlSnapshot snapshot, out EventProviderVersion version)
        {
            var sqlDomainEvents = new Collection<SqlDomainEvent>();
            
            using (var dataReader = command.ExecuteReader())
            {
                // throw exception if no rows
                if (dataReader.HasRows == false)
                {
                    throw new EventProviderNotFoundException();
                }

                // check for event provider descriptor
                if (dataReader.Read() == false)
                {
                    throw new InvalidOperationException("No event provider descriptor result returned.");
                }

                // get event provider information
                GetEventProviderInformation(dataReader, out eventProviderDescriptor, out snapshot, out version);

                // move reader to next result set
                if (dataReader.NextResult())
                {
                    // read until no more rows
                    while (dataReader.Read())
                    {
                        // create new sql domain event
                        var sqlDomainEvent = new SqlDomainEvent(dataReader.GetInt32(0),
                            dataReader.GetInt32(1),
                            dataReader.GetString(2),
                            (byte[])dataReader[3]);

                        // add sql domain event to collection
                        sqlDomainEvents.Add(sqlDomainEvent);
                    }
                }
                else
                {
                    // no events returned
                    throw new InvalidOperationException("No event result returned.");
                }
            }

            // check if snapshot or events were returned
            if (snapshot == null && sqlDomainEvents.Any() == false)
            {
                throw new InvalidOperationException("No snapshot or events returned.");
            }
            
            return sqlDomainEvents;
        }

        private static void GetEventProviderInformation(SqlDataReader dataReader, out EventProviderDescriptor eventProviderDescriptor, out SqlSnapshot snapshot, out EventProviderVersion version)
        {
            // get descriptor
            eventProviderDescriptor = new EventProviderDescriptor(dataReader.GetString(0));

            version = new EventProviderVersion(dataReader.GetInt32(1));

            // read snapshot
            snapshot = GetSnapshot(dataReader);
        }

        private static SqlSnapshot GetSnapshot(SqlDataReader dataReader)
        {
            SqlSnapshot snapshot = null;

            // check if snapshot result is null
            if (dataReader.IsDBNull(2) == false && dataReader.IsDBNull(3) == false)
            {
                // get snapshot
                snapshot = new SqlSnapshot(dataReader.GetString(2), (byte[])dataReader[3]);
            }
            else if (dataReader.IsDBNull(2) == true && dataReader.IsDBNull(3) == false)
            {
                // throw error
                throw new InvalidOperationException("Snapshot type is null but data is not");
            }
            else if (dataReader.IsDBNull(2) == false && dataReader.IsDBNull(3) == true)
            {
                // throw error
                throw new InvalidOperationException("Snapshot data is null but type is not");
            }
            
            return snapshot;
        }

        private static SqlCommand CreateQueryCommand(SqlConnection conn, EventProviderType eventProviderType, Identity identity)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetDomainEvents]", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters
            sqlCommand.Parameters.Add("@eventProviderGuid", SqlDbType.UniqueIdentifier).Value = identity.Value;
            sqlCommand.Parameters.Add("@eventProviderType", SqlDbType.VarChar, 512).Value = eventProviderType.FullName;

            return sqlCommand;
        }

        protected override void CommitTransaction(Transaction transaction)
        {
            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                SqlParameter returnParameter;

                // establish command
                SqlCommand command = CreateWriteCommand(conn, transaction.Command, transaction.EventProviders, transaction.User, out returnParameter);

                // connection needs to be open before executing
                conn.Open();

                // execute
                command.ExecuteNonQuery();

                // check for error
                CheckForError(returnParameter);                
            }            
        }

        private void CheckForError(SqlParameter returnParameter)
        {
            // return value will be 0 for success, any other value will be an error
            if ((int)returnParameter.Value != 0)
            {
                throw new InvalidOperationException(string.Format("Error executing command. Return code: {0}", (int)returnParameter.Value));
            }
        }

        private Snapshot DeserializeSnapshot(SqlSnapshot sqlSnapshot)
        {
            Contract.Requires(sqlSnapshot != null);

            // deserialize snapshot data
            var deserializedObject = _serializer.Deserialize(TypeHelper.Find(sqlSnapshot.FullName), sqlSnapshot.Data, _encoding);

            // make sure object was deserialized
            if (deserializedObject != null)
            {
                return new Snapshot(deserializedObject);
            }

            throw new SnapshotSerializationException("Could not deserialize snapshot data");
        }

        private IDomainEvent DeserializeDomainEvent(string domainEventType, byte[] data)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventType) == false);
            Contract.Requires(data != null);
            Contract.Requires(data.Length > 0);
            Contract.Ensures(Contract.Result<IDomainEvent>() != null);

            // deserialize domain event
            var deserializedObject = _serializer.Deserialize(TypeHelper.Find(domainEventType), data, _encoding);

            // make sure deserialization was successful
            if (deserializedObject == null)
            {
                throw new DomainEventSerializationException(string.Format("Could not deserialize {0}", domainEventType));
            }

            // make sure deserialized object is a domain event
            if (deserializedObject is IDomainEvent)
            {
                return deserializedObject as IDomainEvent;
            }

            // error deserializing domain event
            throw new DomainEventSerializationException("Deserialized object is not a domain event.");
        }

        private byte[] SerializeObject(object objectToSerialize)
        {
            Contract.Requires(objectToSerialize != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length > 0);

            // serialize object to bytes for storage
            var bytes = _encoding.GetBytes(_serializer.Serialize(objectToSerialize));
            Contract.Assume(bytes != null);
            Contract.Assume(bytes.Length > 0);

            return bytes;
        }

        private SqlCommand CreateWriteCommand(SqlConnection conn, ICommand command, EntityCollection<EventProvider> eventProviders, string user, out SqlParameter returnParameter)
        {
            var sqlCommand = new SqlCommand("[dbo].[CreateTransaction]", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var commandType = new TransactionCommandType(command.GetType());

            // set parameters
            sqlCommand.Parameters.Add("@user", SqlDbType.NVarChar, 256).Value = user;
            sqlCommand.Parameters.Add("@commandGuid", SqlDbType.UniqueIdentifier).Value = command.CommandId;
            sqlCommand.Parameters.Add("@commandTypeFullName", SqlDbType.VarChar, 512).Value = commandType.FullName;
            sqlCommand.Parameters.Add("@commandData", SqlDbType.VarBinary).Value = SerializeObject(command);

            // add return value parameter
            returnParameter = sqlCommand.Parameters.Add(new SqlParameter("@retval", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                });

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

        private void GetDataTables(EntityCollection<EventProvider> eventProviders, out DataTable eventProviderDataTable, out DataTable eventDataTable, out DataTable snapshotDataTable)
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
            var snapshot = GetSnapshotIfPolicySatisfied(eventProvider);

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
            dataRow["Data"] = SerializeObject(domainEvent);

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
            dataRow["Data"] = SerializeObject(snapshot.Data);

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
