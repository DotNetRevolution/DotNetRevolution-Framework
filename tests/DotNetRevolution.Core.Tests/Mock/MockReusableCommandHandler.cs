using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override ICommandHandlingResult Handle(TCommand command)
        {
            return new CommandHandlingResult(command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(TCommand command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
