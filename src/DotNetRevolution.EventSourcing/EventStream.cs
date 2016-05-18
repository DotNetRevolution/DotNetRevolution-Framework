using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream : ValueObject<EventStream>, IEnumerable<IDomainEvent>
    {
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEvent>>() != null);

                return _domainEvents;
            }
        }

        public EventStream(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));

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
