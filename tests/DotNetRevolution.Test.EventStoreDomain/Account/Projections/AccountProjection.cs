using DotNetRevolution.EventSourcing.Projecting;
using System.Diagnostics.Contracts;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvent;
using System;
using System.Linq;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class AccountProjection : Projection<AccountProjectionState>,
        IProject<Created>,
        IProject<DebitApplied>,
        IProject<CreditApplied>
    {
        public AccountProjection(ProjectionIdentity identity)
            : base(identity, new AccountProjectionState())
        {
            Contract.Requires(identity != null);
        }

        public void Project(Created domainEvent)
        {
            State.Accounts.Add(new Account(domainEvent.AccountId, domainEvent.Balance));
        }

        public void Project(CreditApplied domainEvent)
        {
            var account = State.Accounts.FirstOrDefault(x => x.Id == domainEvent.AccountId);

            if (account == null)
            {
                throw new ApplicationException("account not found");
            }

            account.Balance = domainEvent.NewBalance;
        }

        public void Project(DebitApplied domainEvent)
        {
            var account = State.Accounts.FirstOrDefault(x => x.Id == domainEvent.AccountId);

            if (account == null)
            {
                throw new ApplicationException("account not found");
            }

            account.Balance = domainEvent.NewBalance;
        }
    }
}
