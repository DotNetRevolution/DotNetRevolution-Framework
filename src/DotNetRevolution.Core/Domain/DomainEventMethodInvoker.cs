using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventMethodInvoker<TDomainEvent> : IDomainEventMethodInvoker
    {
        private readonly Action<TDomainEvent> _action;

        public DomainEventMethodInvoker(Action<TDomainEvent> action)
        {
            Contract.Requires(action != null);

            _action = action;
        }

        public void Invoke(object domainEvent)
        {
            _action((TDomainEvent)domainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_action != null);
        }
    }
}
