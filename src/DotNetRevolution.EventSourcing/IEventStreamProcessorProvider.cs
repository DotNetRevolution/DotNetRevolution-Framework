using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamProcessorProviderContract))]
    public interface IEventStreamProcessorProvider
    {
        void AddProcessor(EventProviderType eventProviderType, IEventStreamProcessor eventStreamProcessor);

        [Pure]
        IEventStreamProcessor GetProcessor(EventProviderType eventProviderType);
    }
}
