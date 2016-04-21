using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerFactory))]
    internal abstract class DomainEventHandlerFactoryContract : IDomainEventHandlerFactory
    {
        public IDomainEventCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainEventCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IDomainEventHandlerCollection GetHandlers(Type domainEventType)
        {
            Contract.Requires(domainEventType != null);
            Contract.Ensures(Contract.Result<IDomainEventHandlerCollection>() != null);
            
            throw new NotImplementedException();
        }
    }
}
