using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamCollection : IReadOnlyCollection<EventProvider>
    {
        private readonly IReadOnlyCollection<EventProvider> _streams;
        
        public int Count
        {
            get
            {
                return _streams.Count;
            }
        }

        public EventStreamCollection(params EventProvider[] streams)
        {
            Contract.Requires(streams != null);
            
            _streams = streams;
        }

        IEnumerator<EventProvider> IEnumerable<EventProvider>.GetEnumerator()
        {
            return _streams.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _streams.GetEnumerator();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_streams != null);
        }
    }
}
