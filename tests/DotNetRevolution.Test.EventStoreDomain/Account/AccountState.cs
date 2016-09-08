using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Reflection;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using DotNetRevolution.Test.EventStoreDomain.Account.Snapshots;
using System.Collections.Generic;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountState : AggregateRootState
    {
        private static IMethodInvoker _methodInvoker = new NamedMethodInvoker<AccountState>("When");
        
        public decimal Balance { get; private set; }

        public AccountState(Created domainEvent)
            : base(new List<IDomainEvent> { domainEvent })
        {
            Apply(domainEvent);
        }
        
        public override void Redirect(object param)
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
