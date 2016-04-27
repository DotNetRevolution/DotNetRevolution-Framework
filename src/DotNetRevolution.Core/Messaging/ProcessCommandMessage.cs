using DotNetRevolution.Core.Commanding;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessCommandMessage : Message
    {
        public ICommand Command { get; }

        public ProcessCommandMessage(ICommand command)
        {
            Contract.Requires(command != null);

            Command = command;
        }
    }
}
