using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageEntryContract))]
    public interface IMessageEntry
    {
        Type MessageType { [Pure] get; }

        Type MessageHandlerType { [Pure] get; }

        IMessageHandler MessageHandler { [Pure] get; set; }
    }
}
