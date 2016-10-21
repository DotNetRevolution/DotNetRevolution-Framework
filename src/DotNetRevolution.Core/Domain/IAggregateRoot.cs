using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootContract))]
    public interface IAggregateRoot
    {
        AggregateRootIdentity Identity { [Pure] get; }

        IAggregateRootState State { [Pure] get; }

        void Execute(ICommand command);
    }
}
