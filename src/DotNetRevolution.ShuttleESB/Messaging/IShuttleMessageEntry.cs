using System;
using System.Diagnostics.Contracts;
using Shuttle.ESB.Core;
using DotNetRevolution.ShuttleESB.Messaging.CodeContract;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    [ContractClass(typeof(ShuttleMessageEntryContract))]
    public interface IShuttleMessageEntry
    {
        Type MessageType { [Pure] get; }
        Type MessageHandlerType { [Pure] get; }
        IMessageHandler MessageHandler { [Pure] get; set; } 
    }
}
