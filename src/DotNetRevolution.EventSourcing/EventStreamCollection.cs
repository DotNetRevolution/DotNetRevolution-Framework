using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamCollection : IReadOnlyCollection<EventStream>
    {
        private readonly IReadOnlyCollection<EventStream> _streams;
        
        public int Count
        {
            get
            {
                return _streams.Count;
            }
        }

        public EventStreamCollection(params EventStream[] streams)
        {
            Contract.Requires(streams != null);
            
            _streams = new List<EventStream>(streams).AsReadOnly();
        }

        IEnumerator<EventStream> IEnumerable<EventStream>.GetEnumerator()
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
