using DotNetRevolution.EventSourcing.Snapshotting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    [ContractClass(typeof(SnapshotPolicyContract))]
    public interface ISnapshotPolicy
    {
        [Pure]
        bool Check(IEventStream eventStream);
    }
}
