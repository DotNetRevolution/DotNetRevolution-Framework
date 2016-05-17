using System;

namespace DotNetRevolution.EventSourcing
{
    public interface IEventStreamProcessorProvider
    {
        void AddProcessor(EventProviderType eventProviderType, IEventStreamProcessor eventStreamProcessor);

        IEventStreamProcessor GetProcessor(EventProviderType eventProviderType);
    }
}
