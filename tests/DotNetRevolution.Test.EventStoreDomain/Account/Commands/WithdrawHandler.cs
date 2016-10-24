using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class WithdrawHandler : CommandHandler<Withdraw>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public WithdrawHandler(IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(domainEventDispatcher != null);

            _domainEventDispatcher = domainEventDispatcher;
        }

        public override ICommandHandlingResult Handle(Withdraw command)
        {
            Contract.Assume(command.Amount > decimal.Zero);

            var domainEvents = AccountAggregateRoot.Create(new Create(Guid.NewGuid(), 50));

            domainEvents.AggregateRoot.Execute(command);

            return new CommandHandlingResult(command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(Withdraw command)
        {
            return Task.Run(() => Handle(command));
        }

        private bool CanDebitAccount(AccountState account, decimal amount, out string declinationReason)
        {
            Contract.Requires(account != null);

            declinationReason = null;

            if (amount <= 0)
            {
                declinationReason = "Cannot debit an acount for $0.00.";
                return false;
            }
            else if (account.Balance == 0)
            {
                declinationReason = "Insufficient funds.";
                return false;
            }
            else if (account.Balance < 0)
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