using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Reflection;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent;
using DotNetRevolution.Test.EventStoreDomain.Account.Snapshots;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountState : AggregateRootState
    {
        private static IMethodInvoker _methodInvoker = new NamedMethodInvoker<AccountState>("When");
        
        public decimal Balance { get; private set; }

        public AccountState(Created domainEvent)
        {
            Apply(domainEvent);
        }

        [UsedImplicitly]
        private AccountState(AccountSnapshot snapshot)
        {
            Contract.Requires(snapshot != null);

            Redirect(snapshot);
        }

        [UsedImplicitly]
        private AccountState(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            Redirect(domainEvents);
        }

        [UsedImplicitly]
        private AccountState(AccountSnapshot snapshot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(snapshot != null);
            Contract.Requires(domainEvents != null);

            Redirect(snapshot);
            Redirect(domainEvents);
        }

        protected override void Redirect(object param)
        {
            _methodInvoker.InvokeMethodFor(this, param);
        }

        private void When(AccountSnapshot snapshot)
        {
            Balance = snapshot.Balance;
        }

        private void When(Created domainEvent)
        {
            Balance = domainEvent.Balance;
        }

        private void When(DebitApplied domainEvent)
        {
            Balance = domainEvent.NewBalance;
        }

        private void When(CreditApplied domainEvent)
        {
            Balance = domainEvent.NewBalance;
        }
    }
}
