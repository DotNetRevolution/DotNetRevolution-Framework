using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(AggregateRootContract))]
    public interface IAggregateRoot
    {
        Identity Identity { [Pure] get; }   
    }
}
