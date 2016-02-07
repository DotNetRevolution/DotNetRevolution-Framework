using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageCatalogContract))]
    public interface IMessageCatalog
    {
        [Pure]
        IMessageEntry GetEntry(Type messageType);

        void Add(IMessageEntry entry);
    }
}
