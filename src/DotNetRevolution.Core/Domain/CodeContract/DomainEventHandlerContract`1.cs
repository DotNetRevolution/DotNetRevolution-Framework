using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IDomainEventHandler<>))]
    internal abstract class DomainEventHandlerContract<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public abstract bool Reusable { get; }

        public void Handle(IDomainEventHandlerContext<TDomainEvent> context)
        {
            Contract.Requires(context != null);
        }

        public abstract  void Handle(IDomainEventHandlerContext context);
    }
}
