using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStreamProcessor<,>))]
    internal abstract class EventStreamProcessorContract<TAggregateRoot, TAggregateRootState> : IEventStreamProcessor<TAggregateRoot, TAggregateRootState>
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        public TAggregateRoot Process(IEventStream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>()?.State != null);

            throw new NotImplementedException();
        }
    }
}
