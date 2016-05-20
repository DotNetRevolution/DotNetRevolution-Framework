using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotProviderFactory))]
    internal abstract class SnapshotProviderFactoryContract : ISnapshotProviderFactory
    {
        public void AddProvider(EventProviderType eventProviderType, ISnapshotProvider snapshotProvider)
        {
            Contract.Requires(eventProviderType != null);
            Contract.Requires(snapshotProvider != null);
            Contract.Ensures(GetProvider(eventProviderType) != null);
        }

        public ISnapshotProvider GetProvider(EventProviderType eventProviderType)
        {
            Contract.Requires(eventProviderType != null);
            
            throw new NotImplementedException();
        }
    }
}
