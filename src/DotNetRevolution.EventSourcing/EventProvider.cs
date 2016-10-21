using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider : IEventProvider
    {
        public EventProviderIdentity EventProviderIdentity { get; }

        public AggregateRootType AggregateRootType { get; }

        public AggregateRootIdentity AggregateRootIdentity { get; }        
        
        public EventProvider(EventProviderIdentity eventProviderIdentity,
            AggregateRootType type,
            AggregateRootIdentity aggregateRootIdentity)
        {            
            Contract.Requires(type != null);
            Contract.Requires(aggregateRootIdentity != null);
            Contract.Requires(eventProviderIdentity != null);

            EventProviderIdentity = eventProviderIdentity;
            AggregateRootType = type;
            AggregateRootIdentity = aggregateRootIdentity;
        }

        public EventProvider(EventProviderIdentity eventProviderIdentity, IDomainEventCollection domainEventCollection)
            : this(eventProviderIdentity,
                   new AggregateRootType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity)
        {
            Contract.Requires(eventProviderIdentity != null);
            Contract.Requires(domainEventCollection?.AggregateRoot?.Identity != null);
        }
    }
}
