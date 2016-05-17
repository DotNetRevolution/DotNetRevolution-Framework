using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.Domain.Account.Delegate;
using DotNetRevolution.Test.Domain.Account.DomainEvents;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.Domain.Account
{
    public class AccountAggregateRoot : IAggregateRoot
    {        
        public Identity Identity { get; }

        public decimal Balance { get; private set; }

        public bool Overdrawn
        {
            get
            {
                return Balance < 0;
            }
        }

        public AccountAggregateRoot()
        {
            Identity = Identity.New();
        }
           
        public IDomainEventCollection Credit(decimal amount, CanCreditAccount canCreditAccount)
        {
            Contract.Requires(canCreditAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canCreditAccount(this, amount, out declinationReason))
            {
                Balance += amount;

                var creditApplied = new CreditApplied(Identity, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<IDomainEvent>
                        {
                            creditApplied,
                            new CreditOverdrawn(Identity, Balance)
                        });
                }

                return new DomainEventCollection(this, creditApplied);
            }

            return new DomainEventCollection(this, new CreditDeclined(Identity, amount, Balance, declinationReason));
        }

        public IDomainEventCollection Debit(decimal amount, CanDebitAccount canDebitAccount)
        {
            Contract.Requires(canDebitAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canDebitAccount(this, amount, out declinationReason))
            {
                Balance -= amount;

                var debitApplied = new DebitApplied(Identity, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<IDomainEvent>
                        {
                            debitApplied,
                            new DebitOverdrawn(Identity, Balance)
                        });
                }

                return new DomainEventCollection(this, debitApplied);
            }

            return new DomainEventCollection(this, new DebitDeclined(Identity, amount, Balance, declinationReason));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(Identity != null);
        }
    }
}
