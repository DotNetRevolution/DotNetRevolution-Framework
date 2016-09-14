using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamStateTracker : IStateTracker, IEventStreamStateTracker
    {
        public IEventStream EventStream { get; }

        public EventStreamStateTracker(IEventStream eventStream)
        {
            Contract.Requires(eventStream != null);

            EventStream = eventStream;
        }

        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Assume(domainEvents != null);

            EventStream.Append(domainEvents);
        }

        public void Apply(IDomainEvent domainEvent)
        {
            Contract.Assume(domainEvent != null);

            EventStream.Append(domainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(EventStream != null);
        }
    }
}
