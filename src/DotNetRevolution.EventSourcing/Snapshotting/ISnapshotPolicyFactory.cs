using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotPolicyFactoryContract))]
    public interface ISnapshotPolicyFactory
    {
        void AddPolicy(EventProviderType eventProviderType, ISnapshotPolicy snapshotPolicy);

        [Pure]
        ISnapshotPolicy GetPolicy(EventProviderType eventProviderType);
    }
}