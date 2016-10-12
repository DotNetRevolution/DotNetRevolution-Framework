using DotNetRevolution.Core.Domain.CodeContract;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootSynchronizerContract))]
    public interface IAggregateRootSynchronizer
    {
        void Exit(IAggregateRootSynchronizationContext context);

        IAggregateRootSynchronizationContext Enter(Type aggregateRootType, Guid aggregateRootId);

        Task<IAggregateRootSynchronizationContext> EnterAsync(Type aggregateRootType, Guid aggregateRootId);
    }
}
