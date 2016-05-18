using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{

    public class EventProvider<TAggregateRoot> : EventProvider
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
            Contract.Requires(domainEventCollection != null);
            Contract.Requires(eventStreamProcessor != null);
        }

        public TAggregateRoot CreateAggregateRoot()
        {
            return _eventStreamProcessor.Process<TAggregateRoot>(DomainEvents);
        }

        public EventProvider CreateNewVersion(IDomainEventCollection domainEvents)
        {
            Contract.Requires(domainEvents != null);

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
    }
}
