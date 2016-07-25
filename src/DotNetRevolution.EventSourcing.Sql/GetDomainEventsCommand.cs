using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class GetDomainEventsCommand
    {
        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly SqlCommand _command;

        private bool _executed;
        private Collection<SqlDomainEvent> _sqlDomainEvents;
        private SqlSnapshot _sqlSnapshot;
        private EventProviderDescriptor _eventProviderDescriptor;
        private EventProviderVersion _eventProviderVersion;

        public GetDomainEventsCommand(SqlSerializer serializer, 
            ITypeFactory typeFactory,
            EventProviderType eventProviderType, 
            Identity identity)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(eventProviderType != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(identity != null);

            _serializer = serializer;
            _typeFactory = typeFactory;
            _command = CreateSqlCommand(eventProviderType, identity);
        }
        
        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;
            
            // execute command
            _sqlDomainEvents = ExecuteSqlCommand(_command, out _eventProviderDescriptor, out _sqlSnapshot, out _eventProviderVersion);

            // set executed so GetResults will return something
            _executed = true;
        }

        public EventStream GetResults(out EventProviderDescriptor eventProviderDescriptor, out EventProviderVersion version, out Snapshot snapshot)
        {
            Contract.Requires(_executed);

            // set output parameters
            eventProviderDescriptor = _eventProviderDescriptor;
            version = _eventProviderVersion;

            // set snapshot       
            snapshot = _sqlSnapshot == null
                ? null
                : _serializer.DeserializeSnapshot(_sqlSnapshot);

            // return deserialized events
            if (_sqlDomainEvents.Any() == false)
            {
                // return event stream with no events, snapshot only
                return new EventStream(new Collection<IDomainEvent>(), snapshot);
            }

            // return event stream with events and snapshot
            return new EventStream(_serializer.DeserializeDomainEvents(_sqlDomainEvents), snapshot);
        }

        private SqlCommand CreateSqlCommand(EventProviderType eventProviderType, Identity identity)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetDomainEvents]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters
            sqlCommand.Parameters.Add("@eventProviderId", SqlDbType.UniqueIdentifier).Value = identity.Value;
            sqlCommand.Parameters.Add("@eventProviderTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(eventProviderType.Type);

            return sqlCommand;
        }

        private static Collection<SqlDomainEvent> ExecuteSqlCommand(SqlCommand command, out EventProviderDescriptor eventProviderDescriptor, out SqlSnapshot snapshot, out EventProviderVersion version)
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
                            (byte[])dataReader[2],
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
                snapshot = new SqlSnapshot((byte[])dataReader[2], (byte[])dataReader[3]);
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
    }
}
