using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class CreateHandler : CommandHandler<Create>
    {
        public override ICommandHandlingResult Handle(ICommandHandlerContext<Create> context)
        {
            var domainEvents = AccountAggregateRoot.Create(context.Command);

            return new CommandHandlingResult(context.Command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<Create> context)
        {
            return Task.Run(() => Handle(context));
        }
    }
}
