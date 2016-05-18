using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStreamProcessorContract))]
    public interface IEventStreamProcessor
    {
        [Pure]
        TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class;
    }
}