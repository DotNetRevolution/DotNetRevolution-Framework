using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class DomainEventRevision : EventStreamRevision
    {
        [Pure]
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        public DomainEventRevision(EventStreamRevisionIdentity identity, EventProviderVersion version, IDomainEvent domainEvent)
            : this(identity, version, new Collection<IDomainEvent> { domainEvent })
        {
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(domainEvent != null);
        }

        public DomainEventRevision(EventStreamRevisionIdentity identity, EventProviderVersion version, IReadOnlyCollection<IDomainEvent> domainEvents)
            : base(identity, version)
        {
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(domainEvents != null);

            DomainEvents = domainEvents;
        }
    }
}
