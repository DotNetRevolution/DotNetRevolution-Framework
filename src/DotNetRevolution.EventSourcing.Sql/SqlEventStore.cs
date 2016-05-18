using DotNetRevolution.Core.Serialization;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using DotNetRevolution.Core.Helper;
using System.Text;
using System.Data;
using System;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.EventSourcing.Sql
{
    public class SqlEventStore : EventStore
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        private readonly string _connectionString;
        private readonly ISerializer _serializer;

        public SqlEventStore(IEventStreamProcessorProvider eventStreamProcessorProvider,
                             ISerializer serializer,
                             string connectionString)
            : base(eventStreamProcessorProvider)
        {
            Contract.Requires(eventStreamProcessorProvider != null);
            Contract.Requires(serializer != null);
            Contract.Requires(string.IsNullOrEmpty(connectionString) == false);
            
            _connectionString = connectionString;
            _serializer = serializer;
        }

        protected override EventStream GetDomainEvents(EventProviderType eventProviderType, Identity identity, out EventProviderVersion version, out EventProviderDescriptor eventProviderDescriptor)
        {
            Collection<SqlDomainEvent> sqlDomainEvents;

            // create connection
            using (var conn = new SqlConnection(_connectionString))
            {
                // establish command
                SqlCommand command = CreateQueryCommand(conn, eventProviderType, identity);

                // connection needs to be open before executing
                conn.Open();

                // execute
                sqlDomainEvents = ExecuteQueryCommand(command, out eventProviderDescriptor);
            }

            // set version output parameter
            version = new EventProviderVersion(sqlDomainEvents.Max(x => x.EventProviderVersion));

            // return deserialized events
            return new EventStream(DeserializeDomainEvents(sqlDomainEvents));
        }

        private IReadOnlyCollection<IDomainEvent> DeserializeDomainEvents(Collection<SqlDomainEvent> sqlDomainEvents)
        {
            return new ReadOnlyCollection<IDomainEvent>(sqlDomainEvents
                .OrderBy(x => x.EventProviderVersion)
                .ThenBy(x => x.Sequence)
                .Select(x =>
                {
                    var deserializedObject = _serializer.Deserialize(TypeHelper.Find(x.EventType), x.Data, _encoding);

                    if (deserializedObject == null)
                    {
                        throw new DomainEventSerializationException(string.Format("Could not deserialize {0}", x.EventType));
                    }

                    if (deserializedObject is IDomainEvent)
                    {
                        return deserializedObject as IDomainEvent;
                    }

                    throw new DomainEventSerializationException("Deserialized object is not a domain event.");
                })
                .ToList());
        }

        private static Collection<SqlDomainEvent> ExecuteQueryCommand(SqlCommand command, out EventProviderDescriptor eventProviderDescriptor)
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

                // get descriptor
                eventProviderDescriptor = new EventProviderDescriptor(dataReader.GetString(0));

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

            // check if any events were returned
            if (sqlDomainEvents.Any() == false)
            {
                throw new InvalidOperationException("No events returned.");
            }
            
            return sqlDomainEvents;
        }

        private static SqlCommand CreateQueryCommand(SqlConnection conn, EventProviderType eventProviderType, Identity identity)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetDomainEvents]", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = identity.Value;
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

        private SqlCommand CreateWriteCommand(SqlConnection conn, ICommand command, EntityCollection<EventProvider> eventProviders, string user, out SqlParameter returnParameter)
        {
            var sqlCommand = new SqlCommand("[dbo].[CreateTransaction]", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            var commandType = new TransactionCommandType(command.GetType());

            // set parameters
            sqlCommand.Parameters.Add("@user", SqlDbType.NVarChar, 256).Value = user;
            sqlCommand.Parameters.Add("@commandGuid", SqlDbType.UniqueIdentifier).Value = command.CommandId;
            sqlCommand.Parameters.Add("@commandTypeFullName", SqlDbType.VarChar, 512).Value = commandType.FullName;
            sqlCommand.Parameters.Add("@commandData", SqlDbType.VarBinary).Value = _encoding.GetBytes(_serializer.Serialize(command));

            // add return value parameter
            returnParameter = sqlCommand.Parameters.Add(new SqlParameter("@retval", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                });

            DataTable eventProviderDataTable;
            DataTable eventDataTable;

            GetDataTables(eventProviders, out eventProviderDataTable, out eventDataTable);

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
            
            return sqlCommand;
        }

        private void GetDataTables(EntityCollection<EventProvider> eventProviders, out DataTable eventProviderDataTable, out DataTable eventDataTable)
        {
            // create data tables
            eventProviderDataTable = CreateEventProviderDataTable();
            eventDataTable = CreateEventDataTable();

            // variable to create temporary ids
            var eventProviderTempId = -1;

            // go through each data provider
            foreach (var eventProvider in eventProviders)
            {
                AddEventProviderRow(eventProviderDataTable, ++eventProviderTempId, eventProvider);

                // new sequence object to keep track of the sequence per event provider
                var sequence = new TransactionEventSequence();
                
                // go through each domain event in the event provider
                foreach (var domainEvent in eventProvider.DomainEvents)
                {
                    AddEventRow(eventDataTable, eventProviderTempId, sequence.Increment(), domainEvent);
                }
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
            dataRow["Data"] = _encoding.GetBytes(_serializer.Serialize(domainEvent));

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
    }
}
