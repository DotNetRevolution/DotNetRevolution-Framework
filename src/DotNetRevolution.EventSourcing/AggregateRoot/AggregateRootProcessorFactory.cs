using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    public class AggregateRootProcessorFactory : IAggregateRootProcessorFactory
    {
        private readonly Dictionary<EventProviderType, IAggregateRootProcessor> _processors = new Dictionary<EventProviderType, IAggregateRootProcessor>();
        private readonly IAggregateRootProcessor _defaultProcessor;

        public AggregateRootProcessorFactory(IAggregateRootProcessor defaultProcessor)
        {
            Contract.Requires(defaultProcessor != null);

            _defaultProcessor = defaultProcessor;
        }

        public void AddProcessor(EventProviderType eventProviderType, IAggregateRootProcessor eventStreamProcessor)
        {
            _processors.Add(eventProviderType, eventStreamProcessor);

            Contract.Assume(GetProcessor(eventProviderType) == eventStreamProcessor);
        }

        public IAggregateRootProcessor GetProcessor(EventProviderType eventProviderType)
        {
            IAggregateRootProcessor processor;

            if (_processors.TryGetValue(eventProviderType, out processor))
            {
                Contract.Assume(processor != null);

                return processor;
            }

            return _defaultProcessor;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processors != null);
            Contract.Invariant(_defaultProcessor != null);
        }
    }
}
