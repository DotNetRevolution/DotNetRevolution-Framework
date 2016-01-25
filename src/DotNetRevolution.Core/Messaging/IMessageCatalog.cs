using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageCatalogContract))]
    public interface IMessageCatalog
    {
        IMessageEntry this[Type messageType] { [Pure] get; }

        void Add(IMessageEntry entry);
    }
}
