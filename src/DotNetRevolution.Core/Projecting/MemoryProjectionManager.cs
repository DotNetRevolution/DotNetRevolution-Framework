using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class MemoryProjectionManager<TProjection, TAggregateRoot> : ProjectionManager<TProjection, TAggregateRoot>
        where TProjection : Projection<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        private readonly HashSet<TransactionIdentity> _processedTransactions = new HashSet<TransactionIdentity>();
        
        public override bool Processed(TransactionIdentity transactionIdentity)
        {
            return _processedTransactions.Contains(transactionIdentity);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedTransactions != null);
        }
    }
}
