using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandHandlingResult : ICommandHandlingResult
    {
        public Guid CommandId { get; }

        public CommandHandlingResult(Guid commandId)
        {
            Contract.Requires(commandId != Guid.Empty);

            CommandId = commandId;
        }
    }
}
