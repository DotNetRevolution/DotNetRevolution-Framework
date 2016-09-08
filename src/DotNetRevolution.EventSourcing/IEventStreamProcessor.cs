using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamProcessorContract<,>))]
    public interface IEventStreamProcessor<TAggregateRoot, TAggregateRootState>
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        TAggregateRoot Process(IEventStream stream);
    }
}
