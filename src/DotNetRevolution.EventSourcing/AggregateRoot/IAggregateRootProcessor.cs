using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.AggregateRoot.CodeContract;
using DotNetRevolution.EventSourcing.Snapshotting;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.AggregateRoot
{
    [ContractClass(typeof(AggregateRootProcessorContract))]
    public interface IAggregateRootProcessor
    {
        [Pure]
        TAggregateRoot Process<TAggregateRoot>(IEventStream eventStream) where TAggregateRoot : class, IAggregateRoot;

        [Pure]
        TAggregateRoot Process<TAggregateRoot>(Snapshot snapshot) where TAggregateRoot : class, IAggregateRoot;
    }
}