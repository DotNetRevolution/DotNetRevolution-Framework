using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider
    {
        public EventProviderType EventProviderType { get; }

        public Identity Identity { get; }

        public EventProviderVersion Version { get; }

        public EventStream DomainEvents { get; }

        public EventProviderDescriptor Descriptor { get; }

        public EventProvider(EventProviderType type,
            Identity identity,
            EventProviderVersion version,
            EventProviderDescriptor descriptor,
            EventStream domainEvents)
        {
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(descriptor != null);
            Contract.Requires(domainEvents != null);

            EventProviderType = type;
            Identity = identity;
            Version = version;
            Descriptor = descriptor;
            DomainEvents = domainEvents;
        }

        public EventProvider(IDomainEventCollection domainEventCollection)
            : this(new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity,
                   EventProviderVersion.Initial,
                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()),
                   new EventStream(domainEventCollection))
        {
            Contract.Requires(domainEventCollection != null);            
        }
    }
}
