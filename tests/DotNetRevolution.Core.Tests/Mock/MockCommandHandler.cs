using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override bool Reusable
        {
            get
            {
                return false;
            }
        }

        public override ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context)
        {
            return new CommandHandlingResult(context.Command.CommandId);
        }

        public override Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context)
        {
            return Task.Run(() => Handle(context));
        }
    }
}
