using DotNetRevolution.Core.Commanding;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStoreDomain.Account.Commands
{
    public class TransferFundsHandler : CommandHandler<TransferFunds>
    {
        public override ICommandHandlingResult Handle(TransferFunds command)
        {
            return new CommandHandlingResult(command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(TransferFunds command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
