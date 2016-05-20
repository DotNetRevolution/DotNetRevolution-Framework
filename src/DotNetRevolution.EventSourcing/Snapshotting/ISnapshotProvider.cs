using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotProviderContract))]
    public interface ISnapshotProvider
    {
        [Pure]
        Snapshot GetSnapshot(IAggregateRoot aggregateRoot);
    }
}
