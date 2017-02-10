using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; }

        protected Command()
        {
            CommandId = Guid.NewGuid();
        }
    }
}
