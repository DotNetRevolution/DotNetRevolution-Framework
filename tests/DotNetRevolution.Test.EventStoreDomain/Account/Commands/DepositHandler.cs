using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.EventStoreDomain.Account.Delegate;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class DepositHandler : CommandHandler<Deposit>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DepositHandler(IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(domainEventDispatcher != null);

            _domainEventDispatcher = domainEventDispatcher;
        }

        public override void Handle(Deposit command)
        {
            Contract.Assume(command.Amount > decimal.Zero);

            var accountAggregate = new AccountAggregateRoot(new Identity(command.AccountId));

            var events = accountAggregate.Credit(command.Amount, new CanCreditAccount(CanCreditAccount));

            Contract.Assume(Contract.ForAll(events, o => o != null));

            _domainEventDispatcher.PublishAll(events);
        }

        private bool CanCreditAccount(AccountAggregateRoot account, decimal amount, out string declinationReason)
        {
            Contract.Requires(account != null);

            declinationReason = null;

            if (amount <= 0)
            {
                declinationReason = "Cannot credit an acount for $0.00.";
                return false;
            }
            else if (account.Balance + amount > 100m)
            {
                declinationReason = "Account will be over $100 and fees will start to apply.";
                return false;
            }

            return true;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_domainEventDispatcher != null);
        }
    }
}
