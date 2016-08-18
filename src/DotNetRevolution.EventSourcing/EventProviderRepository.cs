using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.AggregateRoot;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventProviderRepository<TAggregateRoot> : IEventProviderRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IEventStore _eventStore;        
        private readonly IAggregateRootProcessor _aggregateRootProcessor;

        public EventProviderRepository(IEventStore eventStore,
                                       IAggregateRootProcessor aggregateRootProcessor)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(aggregateRootProcessor != null);

            _eventStore = eventStore;
            _aggregateRootProcessor = aggregateRootProcessor;
        }        
        
        public IEventStream GetByIdentity(Identity identity, out TAggregateRoot aggregateRoot)
        {
            var eventStream = _eventStore.GetEventStream<TAggregateRoot>(identity);

            aggregateRoot = _aggregateRootProcessor.Process<TAggregateRoot>(eventStream);

            return eventStream;
        }

        public void Commit(ICommand command, IEventStream eventStream)
        {   
            var transaction = new EventProviderTransaction(command, eventStream);            

            _eventStore.Commit(transaction);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_aggregateRootProcessor != null);
            Contract.Invariant(_eventStore != null);
        }
    }
}
