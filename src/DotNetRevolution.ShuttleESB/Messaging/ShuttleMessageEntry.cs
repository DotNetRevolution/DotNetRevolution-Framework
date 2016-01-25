using System;
using System.Diagnostics.Contracts;
using Shuttle.ESB.Core;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class ShuttleMessageEntry : IShuttleMessageEntry
    {
        public Type MessageType { get; }
        public Type MessageHandlerType { get; }
        public IMessageHandler MessageHandler { get; set; }

        public ShuttleMessageEntry(Type messageType, Type handlerType)
        {
            Contract.Requires(messageType != null);
            Contract.Requires(handlerType != null);

            MessageType = messageType;
            MessageHandlerType = handlerType;
        }

        public ShuttleMessageEntry(Type messageType, IMessageHandler handler)
        {
            Contract.Requires(messageType != null);
            Contract.Requires(handler != null);

            MessageType = messageType;
            MessageHandlerType = handler.GetType();
            MessageHandler = handler;
        }
    }
}
