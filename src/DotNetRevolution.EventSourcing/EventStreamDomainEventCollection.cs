using System.Collections;
using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamDomainEventCollection : IEventStreamDomainEventCollection
    {
        private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

        public IEventStream EventStream { get; }

        public int Count
        {
            get
            {
                return _domainEvents.Count;
            }
        }

        public EventStreamDomainEventCollection(IEventStream eventStream, IDomainEvent domainEvent)
        {
            Contract.Requires(eventStream != null);
            Contract.Requires(domainEvent != null);

            EventStream = eventStream;
            _domainEvents = new List<IDomainEvent> { domainEvent }.AsReadOnly();
        }

        public EventStreamDomainEventCollection(IEventStream eventStream, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(eventStream != null);
            Contract.Requires(domainEvents != null);

            EventStream = eventStream;
            _domainEvents = domainEvents;
        }

        public EventStreamDomainEventCollection(IEventStream eventStream, params IDomainEvent[] domainEvents)
        {
            Contract.Requires(eventStream != null);
            Contract.Requires(domainEvents != null);

            EventStream = eventStream;
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
        private void ObjectInvariant()
        {
            Contract.Invariant(_domainEvents != null);
        }
    }
}
