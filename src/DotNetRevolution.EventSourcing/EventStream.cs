using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream : IEventStream
    {
        private readonly List<EventStreamRevision> _revisions = new List<EventStreamRevision>();

        public IEventProvider EventProvider { get; }

        public EventStream(IDomainEventCollection domainEvents)
            : this(new EventProvider(domainEvents), new DomainEventRevision(domainEvents))
        {
            Contract.Requires(domainEvents?.AggregateRoot != null);
        }

        public EventStream(IEventProvider eventProvider, IReadOnlyCollection<EventStreamRevision> revisions)
            : this(eventProvider, revisions.AsEnumerable())
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);
        }

        public EventStream(IEventProvider eventProvider, params EventStreamRevision[] revisions)
            : this(eventProvider, revisions.AsEnumerable())
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);
        }

        private EventStream(IEventProvider eventProvider, IEnumerable<EventStreamRevision> revisions)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(revisions != null);            

            EventProvider = eventProvider;
            _revisions.AddRange(revisions);
        }

        public void Append(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            // find latest version
            var latestVersion = GetLatestVersion();

            // create new revision using last revision's version to increment from
            var newRevision = new DomainEventRevision(latestVersion.Increment(), domainEvents);

            // add new revision to stream
            _revisions.Add(newRevision);
        }

        public void Append(IDomainEvent domainEvent)
        {
            Append(new Collection<IDomainEvent> { domainEvent });
        }

        public IReadOnlyCollection<EventStreamRevision> GetUncommittedRevisions()
        {
            return _revisions.Where(x => x.Committed == false)
                             .ToList();
        }

        private EventProviderVersion GetLatestVersion()
        {
            Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

            var orderedRevisions = _revisions.OrderBy(x => x.Version);
            Contract.Assume(orderedRevisions.Count() > 0);

            var lastRevision = orderedRevisions.Last();
            Contract.Assume(lastRevision != null);

            return lastRevision.Version;
        }

        IEnumerator<EventStreamRevision> IEnumerable<EventStreamRevision>.GetEnumerator()
        {
            return _revisions.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _revisions.GetEnumerator();
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_revisions != null);            
        }
    }
}
