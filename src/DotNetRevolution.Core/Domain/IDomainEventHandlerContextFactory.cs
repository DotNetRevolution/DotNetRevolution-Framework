using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerContextFactoryContract))]
    public interface IDomainEventHandlerContextFactory
    {
        [Pure]
        IDomainEventHandlerContext GetContext(IDomainEvent domainEvent);

        [Pure]
        IDomainEventHandlerContext<TDomainEvent> GetContext<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
    }
}
