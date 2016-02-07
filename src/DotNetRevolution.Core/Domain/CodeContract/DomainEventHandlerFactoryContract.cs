using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerFactory))]
    public abstract class DomainEventHandlerFactoryContract : IDomainEventHandlerFactory
    {
        public IDomainEventCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IDomainEventHandlerCollection GetDomainEventHandlers(object domainEvent)
        {
            Contract.Requires(domainEvent != null);
            Contract.Ensures(Contract.Result<IDomainEventHandlerCollection>() != null);
            
            throw new NotImplementedException();
        }
    }
}
