using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootContract<>))]
    public interface IAggregateRoot<TAggregateRootState> : IAggregateRoot
        where TAggregateRootState : IAggregateRootState
    {        
        new TAggregateRootState State { [Pure] get; }
    }
}
