using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider<TAggregateRoot> : EventProvider, IEventProvider<TAggregateRoot>
        where TAggregateRoot : class
    {
        private readonly IEventStreamProcessor _eventStreamProcessor;

        public EventProvider(EventProviderType type,
            Identity identity,
            EventProviderVersion version,
            EventProviderDescriptor descriptor,
            EventStream domainEvents,
            IEventStreamProcessor eventStreamProcessor)
            : base(type, identity, version, descriptor, domainEvents)
        {
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(domainEvents != null);
            Contract.Requires(eventStreamProcessor != null);

            _eventStreamProcessor = eventStreamProcessor;
        }

        public EventProvider(IDomainEventCollection domainEventCollection, IEventStreamProcessor eventStreamProcessor)
            : this(new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity,
                   EventProviderVersion.Initial,
                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()),
                   new EventStream(domainEventCollection),
                   eventStreamProcessor)
        {
            Contract.Requires(domainEventCollection?.AggregateRoot != null);
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventCollection.AggregateRoot.ToString()) == false);
            Contract.Requires(eventStreamProcessor != null);
            Contract.Requires(Contract.ForAll(domainEventCollection, o => o != null));
        }

        public TAggregateRoot CreateAggregateRoot()
        {
            Contract.Assume(Contract.ForAll(DomainEvents, o => o != null));

            return _eventStreamProcessor.Process<TAggregateRoot>(DomainEvents);
        }

        public EventProvider<TAggregateRoot> CreateNewVersion(IDomainEventCollection domainEvents)
        {
            var aggregateRootType = new EventProviderType(domainEvents.AggregateRoot.GetType());            

            if (aggregateRootType != EventProviderType)
            {
                throw new System.Exception();
            }
            
            return new EventProvider<TAggregateRoot>(EventProviderType,
                Identity,
                Version.Increment(),
                new EventProviderDescriptor(domainEvents.AggregateRoot.ToString()),
                new EventStream(domainEvents),
                _eventStreamProcessor);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStreamProcessor != null);
        }
    }
}
