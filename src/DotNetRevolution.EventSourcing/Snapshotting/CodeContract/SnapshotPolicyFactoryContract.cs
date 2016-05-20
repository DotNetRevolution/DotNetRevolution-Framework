using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotPolicyFactory))]
    internal abstract class SnapshotPolicyFactoryContract : ISnapshotPolicyFactory
    {
        public void AddPolicy(EventProviderType eventProviderType, ISnapshotPolicy snapshotPolicy)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Requires(snapshotPolicy != null);
            Contract.Ensures(GetPolicy(eventProviderType) == snapshotPolicy);
        }

        public ISnapshotPolicy GetPolicy(EventProviderType eventProviderType)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Ensures(Contract.Result<ISnapshotPolicy>() != null);

            throw new NotImplementedException();
        }
    }
}
