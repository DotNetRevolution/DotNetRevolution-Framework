using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class MemoryProjectionManager<TProjection> : ProjectionManager
        where TProjection : IProjection
    {
        private readonly HashSet<Guid> _processedTransactions = new HashSet<Guid>();

        public MemoryProjectionManager(IProjectionFactory projectionFactory) 
            : base(projectionFactory)
        {
            Contract.Requires(projectionFactory != null);
        }

        protected override void FinalizeProjection(IProjection projection, EventProviderTransaction transaction)
        {
            _processedTransactions.Add(transaction.Identity);
        }

        protected override void FinalizeProjection(IProjection projection, Exception e)
        {
        }

        protected override void PrepareProjection(IProjection projection)
        {
        }

        protected override bool Processed(TransactionIdentity identity)
        {
            return _processedTransactions.Contains(identity.Value);
        }

        protected override void SaveProjection(IProjection projection)
        {
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedTransactions != null);
        }
    }
}
