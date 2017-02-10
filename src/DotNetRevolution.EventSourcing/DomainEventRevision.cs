using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Collections;

namespace DotNetRevolution.EventSourcing
{
    public class DomainEventRevision : EventStreamRevision, IEnumerable<IDomainEvent>
    {
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

        [Pure]
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

                return _domainEvents;
            }
        }

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

            _domainEvents = domainEvents;
        }

        public IEnumerator<IDomainEvent> GetEnumerator()
        {
            return _domainEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _domainEvents.GetEnumerator();
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_domainEvents != null);
        }
    }
}
