using DotNetRevolution.Core.Command;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockCommandHandler<TCommand> : CommandHandler<TCommand>
        where TCommand : class
    {
        public override bool Reusable
        {
            get
            {
                return false;
            }
        }

        public override void Handle(TCommand command)
        {
        }
    }
}
