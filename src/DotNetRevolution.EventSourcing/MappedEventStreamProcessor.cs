using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public class MappedEventStreamProcessor : EventStreamProcessor
    {
        private readonly EventStreamProcessorMap[] _mappings;

        public MappedEventStreamProcessor(params EventStreamProcessorMap[] mappings)
        {
            Contract.Requires(mappings != null);

            _mappings = mappings;
        }

        protected override string GetMethodName(string domainEventName)
        {
            Contract.Assume(_mappings.Any());

            var mapping = _mappings.First(x => x.DomainEventType.Name == domainEventName);
            Contract.Assume(mapping != null);

            return mapping.MethodName;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_mappings != null);
        }
    }
}
