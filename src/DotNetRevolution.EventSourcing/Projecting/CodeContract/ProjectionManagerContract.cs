using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionManager))]
    internal abstract class ProjectionManagerContract : IProjectionManager
    {
        public IReadOnlyCollection<TransactionIdentity> ProcessedTransactions
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<TransactionIdentity>>() != null);

                throw new NotImplementedException();
            }
        }

        public void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(projectionContexts != null);

            throw new NotImplementedException();
        }
    }
}
