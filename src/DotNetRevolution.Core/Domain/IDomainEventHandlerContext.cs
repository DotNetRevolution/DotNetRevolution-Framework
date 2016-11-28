using DotNetRevolution.Core.Domain.CodeContract;
using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerContextContract))]
    public interface IDomainEventHandlerContext
    {
        [Pure]
        IDomainEvent DomainEvent { get; }

        [Pure]
        MetaCollection Metadata { get; }
    }
}