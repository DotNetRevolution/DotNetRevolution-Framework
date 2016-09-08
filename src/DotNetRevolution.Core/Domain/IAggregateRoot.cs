using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootContract))]
    public interface IAggregateRoot
    {
        Identity Identity { [Pure] get; }

        IAggregateRootState State { [Pure] get; }

        void Execute(ICommand command);
    }
}
