using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; }

        public Command()
        {
            CommandId = Guid.NewGuid();
        }
    }
}
