using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreRepository<TAggregateRoot, TAggregateRootState> : Core.Commanding.Domain.IRepository<TAggregateRoot>
        where TAggregateRootState : class, IAggregateRootState
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventStreamProcessor<TAggregateRoot, TAggregateRootState> _eventStreamProcessor;

        public EventStoreRepository(IEventStore eventStore,
                                    IEventStreamProcessor<TAggregateRoot, TAggregateRootState> eventStreamProcessor)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(eventStreamProcessor != null);

            _eventStore = eventStore;
            _eventStreamProcessor = eventStreamProcessor;
        }

        public async Task<TAggregateRoot> GetByIdentityAsync(AggregateRootIdentity identity)
        {
            Contract.Assume(identity != null);

            var eventStream = await _eventStore.GetEventStreamAsync<TAggregateRoot>(identity);

            return _eventStreamProcessor.Process(eventStream);
        }

        public Task CommitAsync(ICommand command, TAggregateRoot aggregateRoot)
        {
            Contract.Assume(command != null);
            Contract.Assume(aggregateRoot?.State != null);

            var stateTracker = aggregateRoot.State.InternalStateTracker as IEventStreamStateTracker;
            Contract.Assume(stateTracker?.EventStream.GetUncommittedRevisions() != null);
            Contract.Assume(stateTracker?.EventStream.GetUncommittedRevisions().Count > 0);

            TransactionIdentity transactionIdentity = CreateNewTransactionIdentity();

            var transaction = new EventProviderTransaction(command, stateTracker.EventStream, aggregateRoot, transactionIdentity);

            return _eventStore.CommitAsync(transaction);
        }
        
        public TAggregateRoot GetByIdentity(AggregateRootIdentity identity)
        {
            Contract.Assume(identity != null);
                        
            var eventStream = _eventStore.GetEventStream<TAggregateRoot>(identity);
            
            return _eventStreamProcessor.Process(eventStream);
        }

        public void Commit(ICommand command, TAggregateRoot aggregateRoot)
        {
            Contract.Assume(command != null);
            Contract.Assume(aggregateRoot?.State != null);

            var stateTracker = aggregateRoot.State.InternalStateTracker as IEventStreamStateTracker;
            Contract.Assume(stateTracker?.EventStream.GetUncommittedRevisions() != null);
            Contract.Assume(stateTracker?.EventStream.GetUncommittedRevisions().Count > 0);

            TransactionIdentity transactionIdentity = CreateNewTransactionIdentity();

            var transaction = new EventProviderTransaction(command, stateTracker.EventStream, aggregateRoot, transactionIdentity);

            _eventStore.Commit(transaction);
        }

        private TransactionIdentity CreateNewTransactionIdentity()
        {
            Contract.Ensures(Contract.Result<TransactionIdentity>() != null);
            
            var transactionIdentity = new TransactionIdentity(Guid.NewGuid());
            
            return transactionIdentity;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
            Contract.Invariant(_eventStreamProcessor != null);
        }
    }
}
