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
            return _mappings.First(x => x.DomainEventType.Name == domainEventName).MethodName;
        }
    }
}
