using DotNetRevolution.Core.Projecting;
using System.Diagnostics.Contracts;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Projections
{
    public class AccountProjection : Projection,
        IProject<Created>,
        IProject<DebitApplied>,
        IProject<CreditApplied>
    {
        public IList<Account> Accounts { get; }

        public AccountProjection(ProjectionIdentity identity)
            : base(identity)
        {
            Contract.Requires(identity != null);

            Accounts = new List<Account>();
        }

        public void Project(Created domainEvent)
        {
            Accounts.Add(new Account(domainEvent.AccountId, domainEvent.Balance));
        }

        public void Project(CreditApplied domainEvent)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == domainEvent.AccountId);

            if (account == null)
            {
                throw new ApplicationException("account not found");
            }

            account.Balance = domainEvent.NewBalance;
        }

        public void Project(DebitApplied domainEvent)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == domainEvent.AccountId);

            if (account == null)
            {
                throw new ApplicationException("account not found");
            }

            account.Balance = domainEvent.NewBalance;
        }
    }

    public class Account
    {
        public Guid Id { get; }

        public decimal Balance { get; set; }

        public Account(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
        }
    }
}
