using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageCatalogContract))]
    public interface IMessageCatalog
    {
        [Pure]
        IMessageEntry this[Type messageType] { get; }

        void Add(IMessageEntry entry);
    }
}
