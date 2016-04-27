﻿using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageHandlerContract))]
    public interface IMessageHandler
    {
        bool Reusable { [Pure] get; }
        void Handle(IMessage message, string correlationId);
    }

    [ContractClass(typeof(MessageHandlerContract<>))]
    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        void Handle(TMessage message, string correlationId);
    }
}
