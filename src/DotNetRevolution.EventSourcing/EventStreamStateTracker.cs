using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.GuidGeneration;
using System;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamStateTracker : IEventStreamStateTracker
    {
        private readonly Collection<EventStreamRevision> _revisions = new Collection<EventStreamRevision>();
        private readonly IGuidGenerator _guidGenerator;

        public IEventStream EventStream { get; }

        public IReadOnlyCollection<EventStreamRevision> Revisions
        {
            get
            {
                return _revisions;
            }
        }

        public EventStreamStateTracker(IEventStream eventStream,
                                       IGuidGenerator guidGenerator)
        {
            Contract.Requires(eventStream != null);
            Contract.Requires(guidGenerator != null);

            EventStream = eventStream;
            _guidGenerator = guidGenerator;
        }

        public EventStreamStateTracker(IEventStream eventStream,
                                       IGuidGenerator guidGenerator,
                                       IDomainEventCollection domainEvents)
        {
            Contract.Requires(eventStream != null);
            Contract.Requires(guidGenerator != null);
            Contract.Requires(domainEvents != null);

            EventStream = eventStream;
            _guidGenerator = guidGenerator;

            Apply(domainEvents);
        }

        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Assume(domainEvents != null);

            _revisions.Add(new DomainEventRevision(CreateRevisionIdentity(), EventStream.GetNextVersion(), domainEvents));
        }

        public void Apply(IDomainEvent domainEvent)
        {
            Contract.Assume(domainEvent != null);

            _revisions.Add(new DomainEventRevision(CreateRevisionIdentity(), EventStream.GetNextVersion(), domainEvent));
        }

        public void Commit()
        {
            // append revision to event stream
            _revisions.ForEach(EventStream.Append);

            // clear revisions
            _revisions.Clear();
        }

        private EventStreamRevisionIdentity CreateRevisionIdentity()
        {
            Contract.Ensures(Contract.Result<EventStreamRevisionIdentity>() != null);

            var guid = _guidGenerator.Create();
            Contract.Assume(guid != Guid.Empty);

            return new EventStreamRevisionIdentity(guid);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_revisions != null);
            Contract.Invariant(_guidGenerator != null);
        }
    }
}
