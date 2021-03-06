﻿using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Test.Domain.Account.Delegate;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Test.Domain.Account.Commands
{
    public class WithdrawHandler : CommandHandler<Withdraw>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public WithdrawHandler(IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(domainEventDispatcher != null);

            _domainEventDispatcher = domainEventDispatcher;
        }

        public override void Handle(Withdraw command)
        {
            Contract.Assume(command != null);
            Contract.Assume(command.Amount > decimal.Zero);

            var accountAggregate = new AccountAggregateRoot();

            var events = accountAggregate.Debit(command.Amount, new CanDebitAccount(CanDebitAccount));

            Contract.Assume(Contract.ForAll(events, o => o != null));

            _domainEventDispatcher.PublishAll(events);
        }

        private bool CanDebitAccount(AccountAggregateRoot account, decimal amount, out string declinationReason)
        {
            Contract.Requires(account != null);

            declinationReason = null;

            if (amount <= 0)
            {
                declinationReason = "Cannot debit an acount for $0.00.";
                return false;
            }
            else if (account.Overdrawn)
            {
                declinationReason = "Account is overdrawn.";
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