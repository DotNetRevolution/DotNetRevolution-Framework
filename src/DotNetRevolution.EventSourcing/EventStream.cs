using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections;

namespace DotNetRevolution.EventSourcing
{
    public class EventStream : IEventStream
    {
        private readonly List<EventStreamRevision> _revisions = new List<EventStreamRevision>();
        
        public IEventProvider EventProvider { get; }

        public EventProviderVersion LatestVersion { get; private set; }
        
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

            LatestVersion = GetLatestVersion();
        }

        public void Append(EventStreamRevision revision)
        {            
            _revisions.Add(revision);
        }

        public EventProviderVersion GetNextVersion()
        {
            return LatestVersion = LatestVersion.Increment();
        }

        private EventProviderVersion GetLatestVersion()
        {
            Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

            // check for any revisions
            if (_revisions.Any())
            {
                // order revisions by version
                var orderedRevisions = _revisions.OrderBy(x => x.Version);
                Contract.Assume(orderedRevisions.Count() > 0);

                // grab last revision
                var lastRevision = orderedRevisions.Last();
                Contract.Assume(lastRevision != null);

                // return last revision's version
                return lastRevision.Version;
            }
            else
            {
                // no revisions, return new
                return EventProviderVersion.New;
            }
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
