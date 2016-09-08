using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class DomainEventRevision : EventStreamRevision
    {
        [Pure]
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        public DomainEventRevision(IDomainEventCollection domainEvents)
            : this(EventProviderVersion.Initial, domainEvents, false)
        {
            Contract.Requires(domainEvents != null);
        }

        public DomainEventRevision(EventProviderVersion version, IReadOnlyCollection<IDomainEvent> domainEvents)
            : this(version, domainEvents, false)
        {
            Contract.Requires(version != null);
            Contract.Requires(domainEvents != null);
        }

        public DomainEventRevision(EventProviderVersion version, IReadOnlyCollection<IDomainEvent> domainEvents, bool committed)
            : base(version, committed)
        {
            Contract.Requires(version != null);
            Contract.Requires(domainEvents != null);

            DomainEvents = domainEvents;
        }
    }
}
