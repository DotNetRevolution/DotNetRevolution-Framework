using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotPolicy))]
    internal abstract class SnapshotPolicyContract : ISnapshotPolicy
    {
        public bool Check(IEventStream eventStream)
        {
            Contract.Requires(eventStream != null);

            throw new NotImplementedException();
        }
    }
}
