using DotNetRevolution.Core.Commanding;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class DepositHandler : CommandHandler<Deposit>
    {        
        public override ICommandHandlingResult Handle(ICommandHandlerContext<Deposit> context)
        {
            var command = context.Command;

            Contract.Assume(command.Amount > decimal.Zero);

            var domainEvents = AccountAggregateRoot.Create(new Create(Guid.NewGuid(), 50));
            
            domainEvents.AggregateRoot.Execute(command);

            return new CommandHandlingResult(command.CommandId);
        }        

        public override Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<Deposit> context)
        {
            return Task.Run(() => Handle(context));
        }

        private bool CanCreditAccount(AccountState account, decimal amount, out string declinationReason)
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
    }
}
