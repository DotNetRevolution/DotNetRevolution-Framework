using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting.CodeContract
{
    [ContractClassFor(typeof(ISnapshotProvider<>))]
    internal abstract class SnapshotProviderContract<TAggregateRoot> : ISnapshotProvider<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public abstract Snapshot CreateSnapshot(IAggregateRoot aggregateRoot);

        public Snapshot GetSnapshot(TAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Ensures(Contract.Result<object>() != null);

            throw new NotImplementedException();
        }
    }
}
