using DotNetRevolution.Core.Command;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : class
    {
        public override void Handle(TCommand command)
        {
        }
    }
}
