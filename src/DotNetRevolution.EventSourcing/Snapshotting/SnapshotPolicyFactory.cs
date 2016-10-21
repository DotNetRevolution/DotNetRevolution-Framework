using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotPolicyFactory : Dictionary<AggregateRootType, ISnapshotPolicy>, ISnapshotPolicyFactory
    {
        private readonly Dictionary<AggregateRootType, ISnapshotPolicy> _policies = new Dictionary<AggregateRootType, ISnapshotPolicy>();
        private readonly ISnapshotPolicy _defaultPolicy;

        public SnapshotPolicyFactory(ISnapshotPolicy defaultPolicy)
        {
            Contract.Requires(defaultPolicy != null);

            _defaultPolicy = defaultPolicy;
        }

        public void AddPolicy(AggregateRootType eventProviderType, ISnapshotPolicy snapshotPolicy)
        {
            _policies.Add(eventProviderType, snapshotPolicy);

            Contract.Assume(GetPolicy(eventProviderType) == snapshotPolicy);
        }

        public ISnapshotPolicy GetPolicy(AggregateRootType eventProviderType)
        {
            ISnapshotPolicy policy;

            if (_policies.TryGetValue(eventProviderType, out policy))
            {
                Contract.Assume(policy != null);

                return policy;
            }

            return _defaultPolicy;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_policies != null);
            Contract.Invariant(_defaultPolicy != null);
        }
    }
}
