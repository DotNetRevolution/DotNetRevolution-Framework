using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotProviderFactory : ISnapshotProviderFactory
    {
        private readonly Dictionary<AggregateRootType, ISnapshotProvider> _providers = new Dictionary<AggregateRootType, ISnapshotProvider>();
        
        public void AddProvider(AggregateRootType eventProviderType, ISnapshotProvider snapshotProvider)
        {
            _providers.Add(eventProviderType, snapshotProvider);

            Contract.Assume(GetProvider(eventProviderType) == snapshotProvider);
        }

        public ISnapshotProvider GetProvider(AggregateRootType eventProviderType)
        {
            ISnapshotProvider provider;

            if (_providers.TryGetValue(eventProviderType, out provider))
            {                
                return provider;
            }

            return null;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_providers != null);
        }
    }
}
