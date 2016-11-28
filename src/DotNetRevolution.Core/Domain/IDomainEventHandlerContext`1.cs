using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerContextContract<>))]
    public interface IDomainEventHandlerContext<TDomainEvent>  : IDomainEventHandlerContext
        where TDomainEvent : IDomainEvent
    {
        [Pure]
        new TDomainEvent DomainEvent { get; }
    }
}