using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream : ValueObject<EventStream>, IEnumerable<IDomainEvent>
    {        
        public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        public EventStream(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));

            DomainEvents = domainEvents;
        }
     
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(DomainEvents != null);
        }

        public IEnumerator<IDomainEvent> GetEnumerator()
        {
            return DomainEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return DomainEvents.GetEnumerator();
        }
    }
}
