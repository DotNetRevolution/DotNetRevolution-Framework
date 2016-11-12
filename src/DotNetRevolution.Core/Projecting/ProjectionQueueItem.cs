using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    internal class ProjectionQueueItem
    {
        private readonly IEnumerable<IDomainEvent> _domainEvents;

        [Pure]
        public IEnumerable<IDomainEvent> DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<IDomainEvent>>() != null);

                return _domainEvents;
            }
        }

        public ProjectionQueueItem(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            _domainEvents = new Collection<IDomainEvent>
            {
                domainEvent
            };
        }

        public ProjectionQueueItem(IEnumerable<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            _domainEvents = domainEvents;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_domainEvents != null);
        }
    }
}
