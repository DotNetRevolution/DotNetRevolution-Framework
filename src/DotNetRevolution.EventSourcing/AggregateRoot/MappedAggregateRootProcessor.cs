using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public class MappedEventStreamProcessor : AggregateRootProcessor
    {
        private readonly AggregateRootProcessorMap[] _mappings;

        public MappedEventStreamProcessor(params AggregateRootProcessorMap[] mappings)
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
