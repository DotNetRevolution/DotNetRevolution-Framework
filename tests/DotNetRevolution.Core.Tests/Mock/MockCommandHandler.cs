using System;
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

        public override void Handle(TCommand command)
        {
        }

        public override Task HandleAsync(TCommand command)
        {
            return Task.Run(() => Handle(command));
        }
    }
}
