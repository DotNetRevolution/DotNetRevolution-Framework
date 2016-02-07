using DotNetRevolution.Core.Domain.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    [ContractClass(typeof(DomainEventHandlerFactoryContract))]
    public interface IDomainEventHandlerFactory
    {
        IDomainEventCatalog Catalog { [Pure] get; }

        IDomainEventHandlerCollection GetDomainEventHandlers(object domainEvent);
    }
}
