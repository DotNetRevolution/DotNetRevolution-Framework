using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderStateTracker : IEventProviderStateTracker
    {
        private readonly Collection<EventStreamRevision> _revisions = new Collection<EventStreamRevision>();
        private readonly IGuidGenerator _guidGenerator;

        public IEventProvider EventProvider { get; }

        public EventProviderVersion LatestVersion { get; private set; }

        public IReadOnlyCollection<EventStreamRevision> Revisions
        {
            get
            {
                return _revisions;
            }
        }

        public EventProviderStateTracker(IEventProvider eventProvider,
                                         EventProviderVersion latestVersion,
                                         IGuidGenerator guidGenerator)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(latestVersion != null);
            Contract.Requires(guidGenerator != null);

            EventProvider = eventProvider;
            LatestVersion = latestVersion;
            _guidGenerator = guidGenerator;
        }
        
        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Assume(domainEvents != null);

            _revisions.Add(new DomainEventRevision(CreateRevisionIdentity(), LatestVersion = LatestVersion.Increment(), domainEvents));
        }

        public void Apply(IDomainEvent domainEvent)
        {
            Contract.Assume(domainEvent != null);

            _revisions.Add(new DomainEventRevision(CreateRevisionIdentity(), LatestVersion = LatestVersion.Increment(), domainEvent));
        }

        public void Commit()
        {
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
