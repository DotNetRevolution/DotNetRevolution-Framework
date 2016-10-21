using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class CreateHandler : CommandHandler<Create>
    {
        public override ICommandHandlingResult Handle(Create command)
        {
            var domainEvents = AccountAggregateRoot.Create(command);

            return new CommandHandlingResult(command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(Create command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
