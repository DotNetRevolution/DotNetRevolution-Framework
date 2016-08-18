using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider : IEventProvider
    {
        public Identity GlobalIdentity { get; }

        public EventProviderType EventProviderType { get; }

        public Identity Identity { get; }

        public EventProviderDescriptor Descriptor { get; }
        
        public EventProvider(Identity globalIdentity,
            EventProviderType type,
            Identity identity,
            EventProviderDescriptor descriptor)
        {
            Contract.Requires(globalIdentity != null);
            Contract.Requires(type != null);
            Contract.Requires(identity != null);
            Contract.Requires(descriptor != null);

            GlobalIdentity = globalIdentity;
            EventProviderType = type;
            Identity = identity;
            Descriptor = descriptor;
        }

        public EventProvider(IDomainEventCollection domainEventCollection)
            : this(Identity.New(),
                   new EventProviderType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity,
                   new EventProviderDescriptor(domainEventCollection.AggregateRoot.ToString()))
        {
            Contract.Requires(domainEventCollection?.AggregateRoot != null);            
        }
    }
}
