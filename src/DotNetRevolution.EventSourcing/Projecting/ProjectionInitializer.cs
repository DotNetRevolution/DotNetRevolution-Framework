using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionInitializer<TAggregateRoot> : IProjectionInitializer<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private const int DefaultBatchSize = 100;

        private readonly IProjectionDispatcher _dispatcher;
        private readonly IEventStore _eventStore;

        private object _skipLock = new object();
        private int _skip = 0;                
        private int _batchSize = DefaultBatchSize;
        
        public ProjectionInitializer(IEventStore eventStore,
                                     IProjectionDispatcher dispatcher)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(dispatcher != null);

            _eventStore = eventStore;
            _dispatcher = dispatcher;            
        }
        
        public void Initialize()
        {
            Initialize(DefaultBatchSize);
        }

        public void Initialize(int batchSize)
        {                    
            _skip = 0;
            _batchSize = batchSize;

            InitializeAsync(batchSize).Wait();
        }
        
        public Task InitializeAsync()
        {
            return InitializeAsync(DefaultBatchSize);
        }

        public Task InitializeAsync(int batchSize)
        {
            _skip = 0;
            _batchSize = batchSize;

            var processorCount = Environment.ProcessorCount;
            Contract.Assume(processorCount > 0);

            var tasks = new Task[processorCount];

            for (var i = 0; i < processorCount; i++)
            {
                tasks[i] = Task.Run(DoSomethingAsync);
            }
            
            var combinedTasks = Task.WhenAll(tasks);
            Contract.Assume(combinedTasks != null);

            return combinedTasks;
        }    
        
        private async Task DoSomethingAsync()
        {
            ICollection<EventProviderTransactionCollection> transactionCollections;
            int skip;

            skip = GetNextStartingNumber();

            // while transaction collections are being returned, dispatch
            while ((transactionCollections = await _eventStore.GetTransactionsAsync<TAggregateRoot>(skip, _batchSize)).Any())
            {
                skip = GetNextStartingNumber();

                // dispatch transactions for each collection
                transactionCollections.ForEach(x => _dispatcher.Dispatch(x.Transactions.ToArray()));
            }
        }

        private int GetNextStartingNumber()
        {
            int skip;

            lock (_skipLock)
            {
                skip = _skip;

                // increment skip by batch size
                _skip += _batchSize;
            }

            return skip;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
            Contract.Invariant(_skip >= 0);
            Contract.Invariant(_batchSize > 0);
        }
    }
}
