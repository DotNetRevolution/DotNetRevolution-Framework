using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider
    {
        private readonly IEventStreamProcessor _eventStreamProcessor;

        public EventProviderType EventProviderType { get; }

        public Identity Identity { get; }

        public EventProviderVersion Version { get; }

        public EventStream DomainEvents { get; }

        public EventProviderDescriptor Descriptor { get; }

        public EventProvider(EventProviderType type,
            Identity identity,
            EventProviderVersion version,
            EventProviderDescriptor descriptor,
            EventStream domainEvents,
            IEventStreamProcessor eventStreamProcessor)
        {
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(domainEvents != null);
            Contract.Requires(eventStreamProcessor != null);

            EventProviderType = type;
            Identity = identity;
            Version = version;
            Descriptor = descriptor;
            DomainEvents = domainEvents;
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

        public TAggregateRoot CreateAggregateRoot<TAggregateRoot>() where TAggregateRoot : class
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

            return new EventProvider(EventProviderType, 
                Identity, 
                Version.Increment(), 
                new EventProviderDescriptor(domainEvents.AggregateRoot.ToString()),
                new EventStream(domainEvents), 
                _eventStreamProcessor);
        }
    }
}
