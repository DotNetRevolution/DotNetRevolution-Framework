using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{    
    public abstract class AggregateRootWithEventSourcing : AggregateRoot, IAggregateRootWithEventSourcing
    {
        private readonly List<object> _uncommittedEvents = new List<object>();

        public IDomainEventCollection UncommittedEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

                return new DomainEventCollection(this, _uncommittedEvents);
            }
        }

        public AggregateRootWithEventSourcing()
            : base()
        { 
        }

        protected AggregateRootWithEventSourcing(IReadOnlyCollection<object> events)
            : this()
        {
            Contract.Requires(events != null);
            Contract.Assume(Contract.ForAll(events, o => o != null));

            foreach (var domainEvent in events)
            {
                ApplyDomainEvent(domainEvent);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = ".Net event model does not work for domain events.")]
        protected virtual void RaiseDomainEvent(object domainEvent)
        {
            Contract.Requires(domainEvent != null);

            _uncommittedEvents.Add(domainEvent);

            ApplyDomainEvent(domainEvent);
        }

        protected abstract void ApplyDomainEvent(object domainEvent);
        
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_uncommittedEvents != null);
        }
    }
}
