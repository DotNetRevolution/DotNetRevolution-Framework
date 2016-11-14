using System.Threading.Tasks;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override void Handle(TCommand command)
        {
        }

        public override Task HandleAsync(TCommand command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
