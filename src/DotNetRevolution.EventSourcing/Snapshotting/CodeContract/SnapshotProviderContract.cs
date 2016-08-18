using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotProvider))]
    internal abstract class SnapshotProviderContract : ISnapshotProvider
    {
        public Snapshot CreateSnapshot(IAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Ensures(Contract.Result<object>() != null);

            throw new NotImplementedException();
        }        
    }
}
