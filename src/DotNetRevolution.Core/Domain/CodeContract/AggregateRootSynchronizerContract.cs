using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootSynchronizer))]
    internal abstract class AggregateRootSynchronizerContract : IAggregateRootSynchronizer
    {
        public Identity Enter(Type aggregateRootType, Guid id)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<Identity>() != null);

            throw new NotImplementedException();
        }

        public void Exit(Identity identity)
        {
            Contract.Requires(identity != null);

            throw new NotImplementedException();
        }
    }
}
