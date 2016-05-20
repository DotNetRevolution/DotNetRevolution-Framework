using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections;
using DotNetRevolution.EventSourcing.Snapshotting;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream : IEventStream
    {
        private readonly IEventStreamDomainEventCollection _domainEvents;

        public IEventStreamDomainEventCollection DomainEvents
        {
            get
            {
                Contract.Ensures(Contract.Result<IEventStreamDomainEventCollection>() != null);

                return _domainEvents;
            }
        }

        public Snapshot Snapshot { get; }

        public EventStream(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));

            _domainEvents = new EventStreamDomainEventCollection(this, domainEvents);
        }

        public EventStream(IReadOnlyCollection<IDomainEvent> domainEvents, Snapshot snapshot)
            : this(domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));

            Snapshot = snapshot;
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
