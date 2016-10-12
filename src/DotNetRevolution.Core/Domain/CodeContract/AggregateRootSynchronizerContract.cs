using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootSynchronizer))]
    internal abstract class AggregateRootSynchronizerContract : IAggregateRootSynchronizer
    {
        public IAggregateRootSynchronizationContext Enter(Type aggregateRootType, Guid aggregateRootId)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<IAggregateRootSynchronizationContext>() != null);

            throw new NotImplementedException();
        }

        public Task<IAggregateRootSynchronizationContext> EnterAsync(Type aggregateRootType, Guid aggregateRootId)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<Task<IAggregateRootSynchronizationContext>>() != null);

            throw new NotImplementedException();
        }

        public void Exit(IAggregateRootSynchronizationContext context)
        {
            Contract.Requires(context != null);

            throw new NotImplementedException();
        }
    }
}
