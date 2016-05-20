using DotNetRevolution.EventSourcing.AggregateRoot.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    [ContractClass(typeof(AggregateRootProcessorFactoryContract))]
    public interface IAggregateRootProcessorFactory
    {
        void AddProcessor(EventProviderType eventProviderType, IAggregateRootProcessor eventStreamProcessor);

        [Pure]
        IAggregateRootProcessor GetProcessor(EventProviderType eventProviderType);
    }
}
