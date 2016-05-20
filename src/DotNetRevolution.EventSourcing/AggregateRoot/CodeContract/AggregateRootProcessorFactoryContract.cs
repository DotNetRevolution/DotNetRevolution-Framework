using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootProcessorFactory))]
    internal abstract class AggregateRootProcessorFactoryContract : IAggregateRootProcessorFactory
    {
        public void AddProcessor(EventProviderType eventProviderType, IAggregateRootProcessor eventStreamProcessor)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Requires(eventStreamProcessor != null);
            Contract.Ensures(GetProcessor(eventProviderType) == eventStreamProcessor);
        }

        public IAggregateRootProcessor GetProcessor(EventProviderType eventProviderType)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Ensures(Contract.Result<IAggregateRootProcessor>() != null);

            throw new NotImplementedException();
        }
    }
}
