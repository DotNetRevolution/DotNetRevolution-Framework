using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Sql.ReadTransaction
{
    internal class GetTransactionsByAggregateRootTypeCommand
    {
        private readonly SqlSerializer _serializer;
        private readonly ITypeFactory _typeFactory;
        private readonly SqlCommand _command;
        private readonly AggregateRootType _aggregateRootType;
                
        private Collection<SqlDomainEvent> _sqlDomainEvents;
        private Collection<SqlRevision> _revisions;

        public GetTransactionsByAggregateRootTypeCommand(SqlSerializer serializer,
            ITypeFactory typeFactory,
            AggregateRootType aggregateRootType,
            int skip,
            int take)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(aggregateRootType != null);
            Contract.Requires(typeFactory != null);

            _serializer = serializer;
            _typeFactory = typeFactory;
            _aggregateRootType = aggregateRootType;

            _sqlDomainEvents = new Collection<SqlDomainEvent>();
            _revisions = new Collection<SqlRevision>();

            _command = CreateSqlCommand(aggregateRootType, skip, take);
        }

        public ICollection<EventProviderTransactionCollection> Execute(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = _command.ExecuteReader())
            {
                // execute command
                ExecuteSqlCommand(dataReader);
            }

            return GetResult();
        }

        public async Task<ICollection<EventProviderTransactionCollection>> ExecuteAsync(SqlConnection conn)
        {
            Contract.Requires(conn != null);

            // set connection
            _command.Connection = conn;

            using (var dataReader = await _command.ExecuteReaderAsync())
            {                
                // execute command
                ExecuteSqlCommand(dataReader);             
            }

            return GetResult();
        }

        private ICollection<EventProviderTransactionCollection> GetResult()
        {
            var transactionCollectionGroups = _revisions.GroupBy(x => new { x.EventProviderId, x.AggregateRootId }).ToArray();
            
            var transactionCollections = new EventProviderTransactionCollection[transactionCollectionGroups.Length];

            // loop through each event provider creating transactions
            for(var eventProviderGroupIndex = 0; eventProviderGroupIndex < transactionCollections.Length; eventProviderGroupIndex++)
            {
                // get current event provider group
                var eventProviderGroup = transactionCollectionGroups[eventProviderGroupIndex];

                // create transaction array for event provider
                var transactions = new EventProviderTransaction[eventProviderGroup.Select(x => x.TransactionId).Distinct().Count()];

                // filter our event provider's domain events
                var eventProviderEvents = _sqlDomainEvents
                    .Where(x => x.EventProviderId == eventProviderGroup.Key.EventProviderId)
                    .ToArray();

                // order revisions by version then group by transaction
                var eventProviderTransactions = eventProviderGroup
                    .OrderBy(x => x.EventProviderVersion)
                    .GroupBy(x => x.TransactionId)
                    .ToArray();

                // create event provider
                var eventProvider = new EventProvider(eventProviderGroup.Key.EventProviderId, _aggregateRootType, eventProviderGroup.Key.AggregateRootId);

                // add new event provider transaction collection to result
                transactionCollections[eventProviderGroupIndex] = new EventProviderTransactionCollection(eventProvider, transactions); 
                
                // go through each transaction adding revisions and domain events
                for (var eventProviderTransactionIterator = 0; eventProviderTransactionIterator < eventProviderTransactions.Length; eventProviderTransactionIterator++)
                {
                    transactions[eventProviderTransactionIterator] = CreateEventProviderTransaction(eventProvider,
                        eventProviderEvents,
                        eventProviderTransactions[eventProviderTransactionIterator].ToArray());
                }
            }
            
            return transactionCollections;
        }

        private EventProviderTransaction CreateEventProviderTransaction(EventProvider eventProvider, SqlDomainEvent[] eventProviderEvents, SqlRevision[] eventProviderRevisions)
        {
            // create array for transactions revisions
            var revisions = new EventStreamRevision[eventProviderRevisions.Count()];

            SqlRevision revision = null;

            // loop through each revision adding domain events
            for (var revisionIterator = 0; revisionIterator < eventProviderRevisions.Length; revisionIterator++)
            {
                // get current revision
                revision = eventProviderRevisions[revisionIterator];
                
                // add revision to revision collection
                revisions[revisionIterator] = new DomainEventRevision(revision.EventProviderRevisionId,
                    revision.EventProviderVersion,
                    GetRevisionDomainEvents(eventProviderEvents, revision));
            }

            // return new event provider transaction
            return new EventProviderTransaction(revision.TransactionId, eventProvider, _serializer.Deserialize<ICommand>(revision.CommandTypeId, revision.CommandData), new EventProviderDescriptor(revision.Descriptor), revisions, _serializer.Deserialize<List<Meta>>(revision.Metadata));
        }

        private IDomainEvent[] GetRevisionDomainEvents(SqlDomainEvent[] eventProviderEvents, SqlRevision revision)
        {
            // get domain events for this revision ordered by sequence
            var revisionEvents = eventProviderEvents
                                        .Where(x => x.EventProviderRevisionId == revision.EventProviderRevisionId)
                                        .OrderBy(x => x.Sequence)
                                        .ToArray();

            var domainEvents = new IDomainEvent[revisionEvents.Length];

            // loop through each event to add to revision
            for (var revisionEventIterator = 0; revisionEventIterator < revisionEvents.Length; revisionEventIterator++)
            {
                // get current domain event
                var domainEvent = revisionEvents[revisionEventIterator];

                // deserialize domain event and add to collection
                domainEvents[revisionEventIterator] = _serializer.Deserialize<IDomainEvent>(domainEvent.EventTypeId, domainEvent.Data);
            }

            return domainEvents;
        }

        private SqlCommand CreateSqlCommand(AggregateRootType aggregateRootType, int skip, int take)
        {
            var sqlCommand = new SqlCommand("[dbo].[GetTransactionsByAggregateRootType]");
            sqlCommand.CommandType = CommandType.StoredProcedure;

            // set parameters            
            sqlCommand.Parameters.Add("@aggregateRootTypeId", SqlDbType.Binary, 16).Value = _typeFactory.GetHash(aggregateRootType.Type);
            sqlCommand.Parameters.Add("@skip", SqlDbType.Int).Value = skip;
            sqlCommand.Parameters.Add("@take", SqlDbType.Int).Value = take;

            return sqlCommand;
        }

        private void ExecuteSqlCommand(SqlDataReader dataReader)
        {
            Contract.Requires(dataReader != null);
            
            // no transactions read
            if (dataReader.HasRows == false)
            {
                return;
            }

            ReadRevisions(dataReader);

            // move reader to next result set
            if (dataReader.NextResult())
            {
                ReadEvents(dataReader);
            }
            else
            {
                // no events returned
                throw new InvalidOperationException("No event result returned.");
            }
        }

        private void ReadEvents(SqlDataReader dataReader)
        {
            while (dataReader.Read())
            {
                // add sql domain event to collection
                _sqlDomainEvents.Add(new SqlDomainEvent(dataReader.GetGuid(0),
                    dataReader.GetGuid(1),
                    dataReader.GetInt32(2),
                    (byte[])dataReader[3],
                    (byte[])dataReader[4]));
            }
        }

        private void ReadRevisions(SqlDataReader dataReader)
        {
            while (dataReader.Read())
            {
                _revisions.Add(new SqlRevision(dataReader.GetGuid(0),                    
                    dataReader.GetGuid(1),
                    dataReader.GetString(2),
                    dataReader.GetGuid(3),
                    dataReader.GetInt32(4),
                    dataReader.GetGuid(5),
                    (byte[])dataReader[6],                    
                    (byte[])dataReader[7],
                    (byte[])dataReader[8]));
            }
        }
    }
}
