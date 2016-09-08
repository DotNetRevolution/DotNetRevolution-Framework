using DotNetRevolution.Core.Domain.CodeContract;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootStateBuilderContract<>))]
    public interface IAggregateRootStateBuilder<TAggregateRootState> where TAggregateRootState : class, IAggregateRootState
    {
        TAggregateRootState Build(object snapshot);
        TAggregateRootState Build(IReadOnlyCollection<IDomainEvent> domainEvents);
        TAggregateRootState Build(object snapshot, IReadOnlyCollection<IDomainEvent> domainEvents);
    }
}