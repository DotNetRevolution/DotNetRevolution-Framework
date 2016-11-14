using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventDispatcher))]
    internal abstract class DomainEventDispatcherContract : IDomainEventDispatcher
    {
        public void UseCatalog(IDomainEventCatalog catalog)
        {
            Contract.Requires(catalog != null);
        }

        public void Publish(params IDomainEvent[] domainEvents)
        {
            Contract.Requires(domainEvents != null);
        }
    }
}
