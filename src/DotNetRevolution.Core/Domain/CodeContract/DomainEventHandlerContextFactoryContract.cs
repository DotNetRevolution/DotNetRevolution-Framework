using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandlerContextFactory))]
    internal abstract class DomainEventHandlerContextFactoryContract : IDomainEventHandlerContextFactory
    {
        public IDomainEventHandlerContext GetContext(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
            Contract.Ensures(Contract.Result<IDomainEventHandlerContext>() != null);

            throw new NotImplementedException();
        }

        public IDomainEventHandlerContext<TDomainEvent> GetContext<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        {
            Contract.Requires(domainEvent != null);
            Contract.Ensures(Contract.Result<IDomainEventHandlerContext<TDomainEvent>>() != null);

            throw new NotImplementedException();
        }
    }
}
