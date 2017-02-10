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
            var transactionCollections = new Collection<EventProviderTransactionCollection>();

            // loop through each event provider creating transactions
            foreach (var eventProviderGroup in _revisions.GroupBy(x => new { x.EventProviderId, x.AggregateRootId }))
            {
                var transactions = new Collection<EventProviderTransaction>();              
                var eventProvider = new EventProvider(eventProviderGroup.Key.EventProviderId, _aggregateRootType, eventProviderGroup.Key.AggregateRootId);

                // loop through each event providers revisions adding transaction to result
                foreach(var revision in eventProviderGroup)
                {
                    var revisions = new Collection<EventStreamRevision>();                    

                    // loop through each event in revision to add to transaction
                    foreach (var domainEvent in _sqlDomainEvents.Where(x => x.EventProviderRevisionId == revision.EventProviderRevisionId).ToList())
                    {
                        _sqlDomainEvents.Remove(domainEvent);

                        revisions.Add(new DomainEventRevision(revision.EventProviderRevisionId, revision.EventProviderVersion, _serializer.Deserialize<IDomainEvent>(domainEvent.EventTypeId, domainEvent.Data)));
                    }

                    transactions.Add(new EventProviderTransaction(revision.TransactionId, eventProvider, _serializer.Deserialize<ICommand>(revision.CommandTypeId, revision.CommandData), new EventProviderDescriptor(revision.Descriptor), revisions, _serializer.Deserialize<List<Meta>>(revision.Metadata)));
                }

                transactionCollections.Add(new EventProviderTransactionCollection(eventProvider, transactions));
            }
            
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
                    dataReader.GetInt32(1),
                    (byte[])dataReader[2],
                    (byte[])dataReader[3]));
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
