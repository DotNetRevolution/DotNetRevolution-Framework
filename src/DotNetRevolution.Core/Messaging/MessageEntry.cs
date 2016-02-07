using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageEntry : IMessageEntry
    {
        public Type MessageType { get; }
        public Type MessageHandlerType { get; }

        public MessageEntry(Type messageType, Type messageHandlerType)
        {
            Contract.Requires(messageType != null);
            Contract.Requires(messageHandlerType != null);

            MessageType = messageType;
            MessageHandlerType = messageHandlerType;
        }        
    }
}
