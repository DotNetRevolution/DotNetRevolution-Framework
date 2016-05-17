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
            Contract.Requires(eventProviderType != null);
            Contract.Requires(eventStreamProcessor != null);

            _processors.Add(eventProviderType, eventStreamProcessor);
        }

        public IEventStreamProcessor GetProcessor(EventProviderType eventProviderType)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Ensures(Contract.Result<IEventStreamProcessor>() != null);

            IEventStreamProcessor processor;

            if (_processors.TryGetValue(eventProviderType, out processor))
            {
                return processor;
            }

            return _defaultProcessor;
        }
    }
}
