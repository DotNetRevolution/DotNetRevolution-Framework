using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Collections.Generic;
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
        private readonly EventProviderType _eventProviderType;
        private readonly Identity _eventProviderIdentity;

        private bool _executed;

        private SqlSnapshot _sqlSnapshot;

        private Identity _globalIdentity;
        private EventProviderDescriptor _eventProviderDescriptor;

        private Collection<SqlDomainEvent> _sqlDomainEvents;

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
            _eventProviderType = eventProviderType;
            _eventProviderIdentity = identity;

            _command = CreateSqlCommand(eventProviderType, identity);
        }
        
        public void Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;
            
            // execute command
            _sqlDomainEvents = ExecuteSqlCommand(_command);

            // set executed so GetResults will return something
            _executed = true;
        }

        public EventStream GetResults()
        {
            Contract.Requires(_executed);

            var eventProvider = new EventProvider(_globalIdentity, _eventProviderType, _eventProviderIdentity, _eventProviderDescriptor);
            
            // check for snapshot
            if (_sqlSnapshot == null)
            {
                // no snapshot, return event stream with events only
                return new EventStream(eventProvider, GetRevisions());
            }
            else
            {
                // check for events
                if (_sqlDomainEvents.Any() == false)
                {
                    // return event stream with no events, snapshot only
                    return new EventStream(eventProvider, GetSnapshotRevision());
                }

                // get revisions and preprend snapshot revision
                var revisions = GetRevisions();
                revisions.Insert(0, GetSnapshotRevision());

                // return event stream with snapshot and events
                return new EventStream(eventProvider, revisions);
            }            
        }

        private Snapshot GetSnapshot()
        {
            return _sqlSnapshot == null
                ? null
                : _serializer.DeserializeSnapshot(_sqlSnapshot);
        }

        private EventStreamRevision GetSnapshotRevision()
        {
            return new EventStreamRevision(_sqlSnapshot.Version, GetSnapshot(), _sqlSnapshot.Committed);
        }

        private List<EventStreamRevision> GetRevisions()
        {
            var versions = _sqlDomainEvents.GroupBy(x => x.EventProviderVersion);
                                    
            var revisions = new List<EventStreamRevision>(versions.Count());

            foreach(var group in versions)
            {
                var firstDomainEvent = group.First();

                revisions.Add(new EventStreamRevision(firstDomainEvent.EventProviderVersion,
                                                      _serializer.DeserializeDomainEvents(new Collection<SqlDomainEvent>(group.ToList())),
                                                      firstDomainEvent.Committed));
            }
            
            return revisions;
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

        private Collection<SqlDomainEvent> ExecuteSqlCommand(SqlCommand command)
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
                GetEventProviderInformation(dataReader);

                // move reader to next result set
                if (dataReader.NextResult())
                {
                    // read until no more rows
                    while (dataReader.Read())
                    {
                        // create new sql domain event
                        var sqlDomainEvent = new SqlDomainEvent(dataReader.GetInt32(0),
                            dataReader.GetInt32(2),
                            dataReader.GetDateTime(1),
                            (byte[])dataReader[3],
                            (byte[])dataReader[4]);

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
            if (_sqlSnapshot == null && sqlDomainEvents.Any() == false)
            {
                throw new InvalidOperationException("No snapshot or events returned.");
            }

            return sqlDomainEvents;
        }

        private void GetEventProviderInformation(SqlDataReader dataReader)
        {
            // event provider global identity
            _globalIdentity = new Identity(dataReader.GetGuid(0));

            // get descriptor
            _eventProviderDescriptor = new EventProviderDescriptor(dataReader.GetString(1));
            
            // read snapshot
            _sqlSnapshot = GetSnapshot(dataReader);
        }

        private static SqlSnapshot GetSnapshot(SqlDataReader dataReader)
        {
            // load columns in list for easy evaluation
            var snapshotValuesReturnedNull = new List<bool>(4);
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(2));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(3));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(4));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(5));

            // check if snapshot result is null
            if (snapshotValuesReturnedNull.All(x => x == false))
            {
                // get snapshot
                return new SqlSnapshot(new EventProviderVersion(dataReader.GetInt32(2)), dataReader.GetDateTime(5), (byte[])dataReader[3], (byte[])dataReader[4]);
            }
            else if (snapshotValuesReturnedNull.All(x => x == true))
            {
                // return if all values are null
                return null;
            }

            throw new ApplicationException("One or more snapshot values are null.");
        }
    }
}
