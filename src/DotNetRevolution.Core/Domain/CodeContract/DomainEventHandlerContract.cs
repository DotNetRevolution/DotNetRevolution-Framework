using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandler))]
    public abstract class DomainEventHandlerContract : IDomainEventHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(object domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }
    }

    [ContractClassFor(typeof(IDomainEventHandler<>))]
    public abstract class DomainEventHandlerContract<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class
    {
        public abstract bool Reusable { get; }

        public void Handle(TDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);
        }

        public void Handle(object domainEvent)
        {
        }
    }
}
