using System.Diagnostics.Contracts;
using System.Transactions;

namespace DotNetRevolution.Core.Domain
{
    internal class DomainEventEnlistmentNotification : IEnlistmentNotification
    {
        private readonly DomainEventDispatcher _domainEventDispatcher;
        private readonly object _event;

        public DomainEventEnlistmentNotification(DomainEventDispatcher domainEventDispatcher, object domainEvent)
        {
            Contract.Requires(domainEventDispatcher != null);
            Contract.Requires(domainEvent != null);

            _domainEventDispatcher = domainEventDispatcher;
            _event = domainEvent;
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            Contract.Assume(preparingEnlistment != null);

            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            Contract.Assume(enlistment != null);

            _domainEventDispatcher.PublishEvent(_event);

            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            Contract.Assume(enlistment != null);

            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            Contract.Assume(enlistment != null);

            enlistment.Done();
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_event != null);
            Contract.Invariant(_domainEventDispatcher != null);
        }
    }
}
