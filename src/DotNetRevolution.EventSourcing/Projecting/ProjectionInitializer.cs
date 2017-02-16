﻿using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionInitializer : IProjectionInitializer
    {
        private const int DefaultBatchSize = 100;

        private readonly IProjectionDispatcher _dispatcher;
        private readonly IEventStore _eventStore;
        
        public ProjectionInitializer(IEventStore eventStore,
                                     IProjectionDispatcher dispatcher)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(dispatcher != null);

            _eventStore = eventStore;
            _dispatcher = dispatcher;
        }
        
        public void Initialize<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
        {
            Initialize<TAggregateRoot>(DefaultBatchSize);
        }

        public void Initialize<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot
        {                    
            var skip = 0;

            ICollection<EventProviderTransactionCollection> transactionCollections;            
                        
            // while transaction collections are being returned, dispatch
            while ((transactionCollections = _eventStore.GetTransactions<TAggregateRoot>(skip, batchSize)).Any())
            {
                // increment skip by count of collections
                skip += transactionCollections.Count;

                // dispatch transactions for each collection
                transactionCollections.ForEach(x => _dispatcher.Dispatch(x.Transactions.ToArray()));
            }
        }

        public Task InitializeAsync<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot
        {
            return InitializeAsync<TAggregateRoot>(DefaultBatchSize);
        }

        public async Task InitializeAsync<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot
        {
            var skip = 0;

            ICollection<EventProviderTransactionCollection> transactionCollections;

            // while transaction collections are being returned, dispatch
            while ((transactionCollections = await _eventStore.GetTransactionsAsync<TAggregateRoot>(skip, batchSize)).Any())
            {
                // increment skip by count of collections
                skip += transactionCollections.Count;

                // dispatch transactions for each collection
                transactionCollections.ForEach(x => _dispatcher.Dispatch(x.Transactions.ToArray()));
            }
        }        

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
        }
    }
}
