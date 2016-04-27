using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Commanding
{
    [Serializable]
    public class CommandHandlingException : Exception
    {
        public ICommand Command { get; }

        public CommandHandlingException()
        {
        }

        public CommandHandlingException(string message)
            : base(message)
        {
        }

        public CommandHandlingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CommandHandlingException(ICommand command)
            : this(command, null, "Command was not handled correctly.")
        {
            Contract.Requires(command != null);
        }

        public CommandHandlingException(ICommand command, string message)
            : this(command, null, message)
        {
            Contract.Requires(command != null);
        }

        public CommandHandlingException(ICommand command, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(command != null);

            Command = command;
        }

        protected CommandHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            Command = (ICommand) info.GetValue("Command", typeof(ICommand));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Command", Command);

            base.GetObjectData(info, context);
        }
    }
}
