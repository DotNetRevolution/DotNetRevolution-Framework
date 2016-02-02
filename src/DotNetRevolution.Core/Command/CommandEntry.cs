using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    public class CommandEntry : ICommandEntry
    {
        public Type CommandType { get; }
        public Type CommandHandlerType { get; }

        public CommandEntry(Type commandType, Type commandHandlerType)
        {
            Contract.Requires(commandType != null);
            Contract.Requires(commandHandlerType != null);

            CommandType = commandType;
            CommandHandlerType = commandHandlerType;
        }
    }
}
