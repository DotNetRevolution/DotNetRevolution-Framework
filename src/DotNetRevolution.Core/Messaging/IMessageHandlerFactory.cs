using DotNetRevolution.Core.Messaging.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageHandlerFactoryContract))]
    public interface IMessageHandlerFactory
    {
        IMessageCatalog Catalog { [Pure] get; }

        IMessageHandler GetHandler(Type messageType);
    }
}
