using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
                var s1 = new Stopwatch();
                s1.Start();
                // execute command
                ExecuteSqlCommand(dataReader);
                s1.Stop();
                Debug.WriteLine($"s1: {s1.Elapsed}: {_revisions.Count}: {_sqlDomainEvents.Count}");
            }
            var s2 = new Stopwatch();
            s2.Start();
            var results = GetResult();
            s2.Stop();
            Debug.WriteLine($"s2: {s2.Elapsed}: {results.Count}");
            return results;

        }

        private ICollection<EventProviderTransactionCollection> GetResult()
        {
            var transactionCollectionGroups = _revisions.GroupBy(x => new { x.EventProviderId, x.AggregateRootId }).ToList();
            
            var transactionCollections = new List<EventProviderTransactionCollection>(transactionCollectionGroups.Count);

            // loop through each event provider creating transactions
            Parallel.ForEach(transactionCollectionGroups, eventProviderGroup =>
            {
                var eventProviderId = eventProviderGroup.Key.EventProviderId;
                var transactions = new List<EventProviderTransaction>(eventProviderGroup.Select(x => x.TransactionId).Distinct().Count());
                var eventProvider = new EventProvider(eventProviderId, _aggregateRootType, eventProviderGroup.Key.AggregateRootId);

                lock (transactionCollections)
                {
                    transactionCollections.Add(new EventProviderTransactionCollection(eventProvider, transactions));
                }
                
                Guid lastTransactionId = Guid.Empty;
                var sqlEventProviderRevisions = eventProviderGroup.OrderBy(x => x.EventProviderVersion).ToArray();

                var eventProviderEvents = _sqlDomainEvents.Where(x => x.EventProviderId == eventProviderId).ToArray();

                Collection<EventStreamRevision> revisions = null;

                // loop through each event providers revisions adding transaction to result
                for (var revisionIterator = 0; revisionIterator < sqlEventProviderRevisions.Length; revisionIterator++)
                {
                    var revision = sqlEventProviderRevisions[revisionIterator];

                    if (lastTransactionId == Guid.Empty || revision.TransactionId != lastTransactionId)
                    {
                        revisions = new Collection<EventStreamRevision>();
                        transactions.Add(new EventProviderTransaction(revision.TransactionId, eventProvider, _serializer.Deserialize<ICommand>(revision.CommandTypeId, revision.CommandData), new EventProviderDescriptor(revision.Descriptor), revisions, _serializer.Deserialize<List<Meta>>(revision.Metadata)));
                        
                        lastTransactionId = revision.TransactionId;
                    }

                    var domainEvents = new Collection<IDomainEvent>();

                    // loop through each event to add to revision
                    foreach (var domainEvent in eventProviderEvents
                        .Where(x => x.EventProviderRevisionId == revision.EventProviderRevisionId)
                        .OrderBy(x => x.Sequence))
                    {
                        domainEvents.Add(_serializer.Deserialize<IDomainEvent>(domainEvent.EventTypeId, domainEvent.Data));
                    }

                    revisions.Add(new DomainEventRevision(revision.EventProviderRevisionId, revision.EventProviderVersion, domainEvents));
                }
            });
            
            return transactionCollections;
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
