using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotProviderFactoryContract))]
    public interface ISnapshotProviderFactory
    {
        void AddProvider(AggregateRootType eventProviderType, ISnapshotProvider snapshotProvider);

        [Pure]
        ISnapshotProvider GetProvider(AggregateRootType eventProviderType);
    }
}
