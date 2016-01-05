using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class ActionDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent>
        where TDomainEvent : class
    {
        private readonly Action<TDomainEvent> _action;
        
        public ActionDomainEventHandler(Action<TDomainEvent> action)
        {
            Contract.Requires(action != null);

            _action = action;
        }
        
        public override void Handle(TDomainEvent domainEvent)
        {
            _action(domainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_action != null);
        }
    }
}
