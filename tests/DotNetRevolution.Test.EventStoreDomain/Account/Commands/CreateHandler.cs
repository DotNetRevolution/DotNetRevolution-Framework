using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class CreateHandler : CommandHandler<Create>
    {
        public override void Handle(Create command)
        {
            AccountAggregateRoot account;
            var domainEvents = AccountAggregateRoot.Create(command.BeginningBalance, out account);
        }
    }
}
