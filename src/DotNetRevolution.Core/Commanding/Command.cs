using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; }

        public Command(Guid commandId)
        {
            Contract.Requires(commandId != Guid.Empty);

            CommandId = commandId;
        }
    }
}
