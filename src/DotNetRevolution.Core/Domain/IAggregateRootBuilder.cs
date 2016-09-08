using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootBuilderContract<,>))]
    public interface IAggregateRootBuilder<TAggregateRoot, TAggregateRootState>
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        TAggregateRoot Build(Identity identity, TAggregateRootState state);
    }
}