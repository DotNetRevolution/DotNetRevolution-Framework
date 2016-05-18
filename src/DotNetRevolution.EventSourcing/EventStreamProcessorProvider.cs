using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamProcessorProvider : IEventStreamProcessorProvider
    {
        private readonly Dictionary<EventProviderType, IEventStreamProcessor> _processors = new Dictionary<EventProviderType, IEventStreamProcessor>();
        private readonly IEventStreamProcessor _defaultProcessor;
        
        public EventStreamProcessorProvider(IEventStreamProcessor defaultProcessor)
        {
            Contract.Requires(defaultProcessor != null);

            _defaultProcessor = defaultProcessor;
        }

        public void AddProcessor(EventProviderType eventProviderType, IEventStreamProcessor eventStreamProcessor)
        {            
            _processors.Add(eventProviderType, eventStreamProcessor);

            Contract.Assume(GetProcessor(eventProviderType) == eventStreamProcessor);
        }

        public IEventStreamProcessor GetProcessor(EventProviderType eventProviderType)
        {
            IEventStreamProcessor processor;

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
