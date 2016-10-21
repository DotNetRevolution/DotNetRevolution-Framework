using DotNetRevolution.Core.Commanding;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessCommandMessage : Message
    {
        public ICommand Command { get; }

        public ProcessCommandMessage(Guid messageId, ICommand command)
            : base(messageId)
        {
            Contract.Requires(messageId != Guid.Empty);
            Contract.Requires(command != null);

            Command = command;
        }
    }
}
