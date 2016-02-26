using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessCommandMessage
    {
        public object Command { get; }

        public ProcessCommandMessage(object command)
        {
            Contract.Requires(command != null);

            Command = command;
        }
    }
}
