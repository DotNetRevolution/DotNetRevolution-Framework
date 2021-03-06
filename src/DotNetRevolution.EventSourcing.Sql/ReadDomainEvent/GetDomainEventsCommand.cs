﻿using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing.Snapshotting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Sql.ReadDomainEvent
{
    internal class GetDomainEventsCommand
    {
        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly SqlCommand _command;
        private readonly AggregateRootType _aggregateRootType;
        private readonly AggregateRootIdentity _aggregateRootIdentity;
        
        private SqlSnapshot _sqlSnapshot;
        private EventProviderIdentity _eventProviderIdentity;
        private Collection<SqlDomainEvent> _sqlDomainEvents;

        public GetDomainEventsCommand(SqlSerializer serializer, 
            ITypeFactory typeFactory,
            AggregateRootType aggregateRootType, 
            AggregateRootIdentity aggregateRootIdentity)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(aggregateRootType != null);
            Contract.Requires(typeFactory != null);
            Contract.Requires(aggregateRootIdentity != null);

            _serializer = serializer;
            _typeFactory = typeFactory;
            _aggregateRootType = aggregateRootType;
            _aggregateRootIdentity = aggregateRootIdentity;

            _command = CreateSqlCommand(aggregateRootType, aggregateRootIdentity);
        }

        public EventStream Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = _command.ExecuteReader())
            {
                // execute command
                _sqlDomainEvents = ExecuteSqlCommand(dataReader);
            }
            
            return GetResult();
        }

        public async Task<EventStream> ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = await _command.ExecuteReaderAsync())
            {
                // execute command
                _sqlDomainEvents = ExecuteSqlCommand(dataReader);
            }

            return GetResult();
        }

        private EventStream GetResult()
        {
            var eventProvider = new EventProvider(_eventProviderIdentity, _aggregateRootType, _aggregateRootIdentity);
            
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

                // get revisions and prepend snapshot revision
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
            return new SnapshotRevision(_sqlSnapshot.Identity, _sqlSnapshot.Version, GetSnapshot());
        }

        private List<EventStreamRevision> GetRevisions()
        {
            var versions = _sqlDomainEvents.GroupBy(x => new { x.EventProviderRevisionId, x.EventProviderVersion });
                                    
            var revisions = new List<EventStreamRevision>(versions.Count());

            foreach(var group in versions)
            {
                revisions.Add(new DomainEventRevision(group.Key.EventProviderRevisionId,
                                                      group.Key.EventProviderVersion,
                                                      _serializer.DeserializeDomainEvents(new Collection<SqlDomainEvent>(group.ToList()))));
            }
            
            return revisions;
        }

        private SqlCommand CreateSqlCommand(AggregateRootType aggregateRootType, AggregateRootIdentity identity)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetDomainEvents]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters
            sqlCommand.Parameters.Add("@aggregateRootId", SqlDbType.UniqueIdentifier).Value = identity.Value;
            sqlCommand.Parameters.Add("@aggregateRootTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(aggregateRootType.Type);

            return sqlCommand;
        }

        private Collection<SqlDomainEvent> ExecuteSqlCommand(SqlDataReader dataReader)
        {
            Contract.Requires(dataReader != null);

            var sqlDomainEvents = new Collection<SqlDomainEvent>();
            
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
                GetEventProviderEvents(dataReader, sqlDomainEvents);
            }
            else
            {
                // no events returned
                throw new InvalidOperationException("No event result returned.");
            }

            // check if snapshot or events were returned
            if (_sqlSnapshot == null && sqlDomainEvents.Any() == false)
            {
                throw new InvalidOperationException("No snapshot or events returned.");
            }

            return sqlDomainEvents;
        }

        private static void GetEventProviderEvents(SqlDataReader dataReader, Collection<SqlDomainEvent> sqlDomainEvents)
        {
            // read until no more rows
            while (dataReader.Read())
            {
                // add sql domain event to collection
                sqlDomainEvents.Add(new SqlDomainEvent(dataReader.GetGuid(0),
                    dataReader.GetInt32(1),
                    dataReader.GetInt32(2),
                    (byte[])dataReader[3],
                    (byte[])dataReader[4]));
            }
        }

        private void GetEventProviderInformation(SqlDataReader dataReader)
        {
            // event provider global identity
            _eventProviderIdentity = new EventProviderIdentity(dataReader.GetGuid(0));
                        
            // read snapshot
            _sqlSnapshot = GetSnapshot(dataReader);
        }

        private static SqlSnapshot GetSnapshot(SqlDataReader dataReader)
        {
            // load columns in list for easy evaluation
            var snapshotValuesReturnedNull = new List<bool>(4);            
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(1));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(2));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(3));
            snapshotValuesReturnedNull.Add(dataReader.IsDBNull(4));

            // check if snapshot result is null
            if (snapshotValuesReturnedNull.All(x => x == false))
            {
                // get snapshot
                return new SqlSnapshot(dataReader.GetGuid(4), new EventProviderVersion(dataReader.GetInt32(1)), (byte[])dataReader[2], (byte[])dataReader[3]);
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
