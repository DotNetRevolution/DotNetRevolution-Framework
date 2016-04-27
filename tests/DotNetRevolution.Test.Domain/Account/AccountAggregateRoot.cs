using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.Domain.Account.Delegate;
using DotNetRevolution.Test.Domain.Account.DomainEvents;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.Domain.Account
{
    public class AccountAggregateRoot
    {
        private Identity _identity = Identity.New();

        public Identity Identity
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<Identity>() != null);

                return _identity;
            }
        }

        public decimal Balance { get; private set; }

        public bool Overdrawn
        {
            get
            {
                return Balance < 0;
            }
        }
        
        public IDomainEventCollection Credit(decimal amount, CanCreditAccount canCreditAccount)
        {
            Contract.Requires(canCreditAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canCreditAccount(this, amount, out declinationReason))
            {
                Balance += amount;

                var creditApplied = new CreditApplied(_identity.Value, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<IDomainEvent>
                        {
                            creditApplied,
                            new CreditOverdrawn(_identity.Value, Balance)
                        });
                }

                return new DomainEventCollection(this, creditApplied);
            }

            return new DomainEventCollection(this, new CreditDeclined(_identity.Value, amount, Balance, declinationReason));
        }

        public IDomainEventCollection Debit(decimal amount, CanDebitAccount canDebitAccount)
        {
            Contract.Requires(canDebitAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canDebitAccount(this, amount, out declinationReason))
            {
                Balance -= amount;

                var debitApplied = new DebitApplied(_identity.Value, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<IDomainEvent>
                        {
                            debitApplied,
                            new DebitOverdrawn(_identity.Value, Balance)
                        });
                }

                return new DomainEventCollection(this, debitApplied);
            }

            return new DomainEventCollection(this, new DebitDeclined(_identity.Value, amount, Balance, declinationReason));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
        }
    }
}
