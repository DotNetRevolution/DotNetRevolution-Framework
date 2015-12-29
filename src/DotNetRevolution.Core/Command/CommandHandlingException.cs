using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace DotNetRevolution.Core.Command
{
    [Serializable]
    public class CommandHandlingException : Exception
    {
        public object Command { get; private set; }

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

        public CommandHandlingException(object command)
            : this(command, null, "Command was not handled correctly.")
        {
            Contract.Requires(command != null);
        }

        public CommandHandlingException(object command, string message)
            : this(command, null, message)
        {
            Contract.Requires(command != null);
        }

        public CommandHandlingException(object command, Exception innerException, string message)
            : base(message, innerException)
        {
            Contract.Requires(command != null);

            Command = command;
        }

        protected CommandHandlingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            Command = info.GetValue("Command", typeof(object));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Command", Command);

            base.GetObjectData(info, context);
        }
    }
}