using DotNetRevolution.Core.Commanding;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFundsHandler : CommandHandler<TransferFunds>
    {
        public override ICommandHandlingResult Handle(ICommandHandlerContext<TransferFunds> context)
        {
            return new CommandHandlingResult(context.Command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TransferFunds> context)
        {
            return Task.Run(() => Handle(context));
        }
    }
}
