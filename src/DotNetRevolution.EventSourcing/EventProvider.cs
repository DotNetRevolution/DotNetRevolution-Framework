using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider : IEventProvider
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
            Contract.Requires(domainEventCollection?.AggregateRoot != null);
            Contract.Requires(string.IsNullOrWhiteSpace(domainEventCollection.AggregateRoot.ToString()) == false);
            Contract.Requires(Contract.ForAll(domainEventCollection, o => o != null));
        }

        public virtual Snapshot GetSnapshot()
        {
            return null;
        }
    }
}
