using DotNetRevolution.Core.Domain.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootSynchronizerContract))]
    public interface IAggregateRootSynchronizer
    {
        void Exit(Identity identity);

        Identity Enter(Type aggregateRootType, Guid id);
    }
}
