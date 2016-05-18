using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStreamProcessorProvider))]
    internal abstract class EventStreamProcessorProviderContract : IEventStreamProcessorProvider
    {
        public void AddProcessor(EventProviderType eventProviderType, IEventStreamProcessor eventStreamProcessor)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Requires(eventStreamProcessor != null);
            Contract.Ensures(GetProcessor(eventProviderType) == eventStreamProcessor);
        }

        public IEventStreamProcessor GetProcessor(EventProviderType eventProviderType)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Ensures(Contract.Result<IEventStreamProcessor>() != null);

            throw new NotImplementedException();
        }
    }
}
