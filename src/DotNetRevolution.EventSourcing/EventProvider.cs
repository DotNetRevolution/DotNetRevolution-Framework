using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProvider : ValueObject<EventProvider>, IEventProvider
    {
        public EventProviderIdentity Identity { get; }

        public AggregateRootType AggregateRootType { get; }

        public AggregateRootIdentity AggregateRootIdentity { get; }        
        
        public EventProvider(EventProviderIdentity identity,
            AggregateRootType type,
            AggregateRootIdentity aggregateRootIdentity)
        {            
            Contract.Requires(type != null);
            Contract.Requires(aggregateRootIdentity != null);
            Contract.Requires(identity != null);

            Identity = identity;
            AggregateRootType = type;
            AggregateRootIdentity = aggregateRootIdentity;
        }

        public EventProvider(EventProviderIdentity identity, IDomainEventCollection domainEventCollection)
            : this(identity,
                   new AggregateRootType(domainEventCollection.AggregateRoot.GetType()),
                   domainEventCollection.AggregateRoot.Identity)
        {
            Contract.Requires(identity != null);
            Contract.Requires(domainEventCollection?.AggregateRoot?.Identity != null);
        }
    }
}
