using System.Collections.Generic;
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

        public void Publish(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }
        
        public void PublishAll(IEnumerable<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Requires(Contract.ForAll(domainEvents, o => o != null));
        }
    }
}
