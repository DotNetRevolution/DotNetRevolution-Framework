using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStreamProcessor))]
    internal abstract class EventStreamProcessorContract : IEventStreamProcessor
    {
        public TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class
        {
            Contract.Requires(eventStream != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }
    }
}
