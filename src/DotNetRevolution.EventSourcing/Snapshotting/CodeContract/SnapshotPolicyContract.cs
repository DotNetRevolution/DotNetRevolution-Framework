using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotPolicy))]
    internal abstract class SnapshotPolicyContract : ISnapshotPolicy
    {
        public bool Check(IEventProvider eventProvider)
        {
            Contract.Requires(eventProvider != null);

            throw new NotImplementedException();
        }
    }
}
