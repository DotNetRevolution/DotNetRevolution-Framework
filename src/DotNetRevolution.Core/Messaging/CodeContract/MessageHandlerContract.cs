﻿using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageHandler))]
    public abstract class MessageHandlerContract : IMessageHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(object message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }

    [ContractClassFor(typeof(IMessageHandler<>))]
    public abstract class MessageHandlerContract<TMessage> : IMessageHandler<TMessage>
        where TMessage : class
    {
        public abstract bool Reusable { get; }

        public void Handle(object message, string correlationId)
        {
        }

        public void Handle(TMessage message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }
}
