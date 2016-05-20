using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotProviderContract<>))]
    public interface ISnapshotProvider<TAggregateRoot> : ISnapshotProvider
        where TAggregateRoot : class, IAggregateRoot
    {
        [Pure]
        Snapshot GetSnapshot(TAggregateRoot aggregateRoot);
    }
}
