﻿using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.EventStoreDomain.Account.Delegate;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using DotNetRevolution.Test.EventStoreDomain.Account.Exceptions;
using DotNetRevolution.Test.EventStoreDomain.Account.Snapshots;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountAggregateRoot : IAggregateRoot
    {
        public Identity Identity { get; private set; }

        public decimal Balance { get; private set; }
        
        [UsedImplicitly]
        private AccountAggregateRoot()
        {
        }

        public AccountAggregateRoot(AccountSnapshot snapshot)
            : this(new Identity(snapshot.Id))
        {
            Contract.Requires(snapshot != null);
            
            Balance = snapshot.Balance;
        }

        public AccountAggregateRoot(Identity identity)
        {
            Contract.Requires(identity != null);

            Identity = identity;
        }

        public override string ToString()
        {
            return string.Format("AccountID: {0}", Identity.Value.ToString());
        }

        public IDomainEventCollection Credit(decimal amount, CanCreditAccount canCreditAccount)
        {
            Contract.Requires(canCreditAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canCreditAccount(this, amount, out declinationReason))
            {
                var result = new CreditApplied(Identity, amount, Balance + amount);

                Apply(result);

                return new DomainEventCollection(this, result);
            }

            throw new CreditDeclinedException(declinationReason);            
        }

        public IDomainEventCollection Debit(decimal amount, CanDebitAccount canDebitAccount)
        {
            Contract.Requires(canDebitAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canDebitAccount(this, amount, out declinationReason))
            {
                var result = new DebitApplied(Identity, amount, Balance - amount);

                Apply(result);

                return new DomainEventCollection(this, result);
            }

            throw new DebitDeclinedException(declinationReason);
        }

        public static IDomainEventCollection Create(decimal beginningBalance, out AccountAggregateRoot account)
        {
            account = new AccountAggregateRoot();

            var result = new Created(Identity.New(), beginningBalance);

            account.Apply(result);

            return new DomainEventCollection(account, result);                
        }

        private void Apply(Created domainEvent)
        {
            Identity = domainEvent.AccountId;
            Balance = domainEvent.Balance;
        }

        private void OnCreated(Created domainEvent)
        {
            Identity = domainEvent.AccountId;
            Balance = domainEvent.Balance;
        }

        private void Apply(DebitApplied domainEvent)
        {
            Balance = domainEvent.NewBalance;
        }

        private void Apply(CreditApplied domainEvent)
        {
            Balance = domainEvent.NewBalance;
        }        
    }
}
