using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.Domain.Account.Delegate;
using DotNetRevolution.Test.Domain.Account.DomainEvent;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.Domain.Account
{
    public class AccountAggregateRoot : AggregateRoot
    {
        private readonly Identity _identity = Identity.New();
        
        public override Identity Identity
        {
            get
            {
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

                var creditApplied = new CreditApplied(_identity.Id, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<object>
                        {
                            creditApplied,
                            new CreditOverdrawn(_identity.Id, Balance)
                        });
                }

                return new DomainEventCollection(this, creditApplied);
            }

            return new DomainEventCollection(this, new CreditDeclined(_identity.Id, amount, Balance, declinationReason));
        }

        public IDomainEventCollection Debit(decimal amount, CanDebitAccount canDebitAccount)
        {
            Contract.Requires(canDebitAccount != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            string declinationReason;

            if (canDebitAccount(this, amount, out declinationReason))
            {
                Balance -= amount;

                var debitApplied = new DebitApplied(_identity.Id, amount, Balance);

                if (Overdrawn)
                {
                    return new DomainEventCollection(this, new Collection<object>
                        {
                            debitApplied,
                            new DebitOverdrawn(_identity.Id, Balance)
                        });
                }

                return new DomainEventCollection(this, debitApplied);
            }

            return new DomainEventCollection(this, new DebitDeclined(_identity.Id, amount, Balance, declinationReason));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identity != null);
        }
    }
}
