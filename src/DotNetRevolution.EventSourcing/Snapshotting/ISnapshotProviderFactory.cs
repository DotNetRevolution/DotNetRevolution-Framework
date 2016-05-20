using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotProviderFactoryContract))]
    public interface ISnapshotProviderFactory
    {
        void AddProvider(EventProviderType eventProviderType, ISnapshotProvider snapshotProvider);

        [Pure]
        ISnapshotProvider GetProvider(EventProviderType eventProviderType);
    }
}
