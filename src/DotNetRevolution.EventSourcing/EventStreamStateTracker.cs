using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamStateTracker : IStateTracker
    {
        private readonly IEventStream _eventStream;

        public EventStreamStateTracker(IEventStream eventStream)
        {
            Contract.Requires(eventStream != null);

            _eventStream = eventStream;
        }

        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            _eventStream.Append(domainEvents);
        }

        public void Apply(IDomainEvent domainEvent)
        {
            _eventStream.Append(domainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStream != null);
        }
    }
}
