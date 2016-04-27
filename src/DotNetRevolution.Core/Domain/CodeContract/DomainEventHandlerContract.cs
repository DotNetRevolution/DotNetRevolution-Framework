using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandler))]
    internal abstract class DomainEventHandlerContract : IDomainEventHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }
    }

    [ContractClassFor(typeof(IDomainEventHandler<>))]
    internal abstract class DomainEventHandlerContract<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public abstract bool Reusable { get; }

        public void Handle(TDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }

        public void Handle(IDomainEvent domainEvent)
        {
        }
    }
}
