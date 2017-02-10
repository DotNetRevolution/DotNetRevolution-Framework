using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventDispatcher))]
    internal abstract class DomainEventDispatcherContract : IDomainEventDispatcher
    {
        public void Publish(params IDomainEventHandlerContext[] contexts)
        {
            Contract.Requires(contexts != null);
        }

        public void Publish(params IDomainEvent[] domainEvents)
        {
            Contract.Requires(domainEvents != null);
        }
    }
}
