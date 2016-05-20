using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotPolicyFactory : Dictionary<EventProviderType, ISnapshotPolicy>, ISnapshotPolicyFactory
    {
        private readonly Dictionary<EventProviderType, ISnapshotPolicy> _policies = new Dictionary<EventProviderType, ISnapshotPolicy>();
        private readonly ISnapshotPolicy _defaultPolicy;

        public SnapshotPolicyFactory(ISnapshotPolicy defaultPolicy)
        {
            Contract.Requires(defaultPolicy != null);

            _defaultPolicy = defaultPolicy;
        }

        public void AddPolicy(EventProviderType eventProviderType, ISnapshotPolicy snapshotPolicy)
        {
            _policies.Add(eventProviderType, snapshotPolicy);

            Contract.Assume(GetPolicy(eventProviderType) == snapshotPolicy);
        }

        public ISnapshotPolicy GetPolicy(EventProviderType eventProviderType)
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
