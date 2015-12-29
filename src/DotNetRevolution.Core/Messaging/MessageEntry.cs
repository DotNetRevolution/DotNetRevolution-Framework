using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageEntry : IMessageEntry
    {
        public Type MessageType { get; }
        public Type MessageHandlerType { get; }
        public IMessageHandler MessageHandler { get; set; }

        public MessageEntry(Type messageType, Type messageHandlerType)
        {
            Contract.Requires(messageType != null);
            Contract.Requires(messageHandlerType != null);

            MessageType = messageType;
            MessageHandlerType = messageHandlerType;
        }

        public MessageEntry(Type messageType, IMessageHandler messageHandler)
        {
            Contract.Requires(messageType != null);
            Contract.Requires(messageHandler != null);

            MessageType = messageType;
            MessageHandler = messageHandler;
            MessageHandlerType = messageHandler.GetType();
        }
    }
}