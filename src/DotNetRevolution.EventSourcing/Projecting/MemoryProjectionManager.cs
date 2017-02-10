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

        //protected override void Processed(TransactionIdentity transactionIdentity)
        //{
        //    _processedTransactions.Add(transactionIdentity);
        //}

        //protected override bool HasProcessed(TransactionIdentity identity)
        //{
        //    return _processedTransactions.Contains(identity.Value);
        //}
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedTransactions != null);
        }
    }
}
