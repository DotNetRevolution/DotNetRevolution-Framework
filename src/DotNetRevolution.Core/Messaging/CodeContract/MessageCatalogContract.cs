using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageCatalog))]
    internal abstract class MessageCatalogContract : IMessageCatalog
    {
        public IMessageEntry GetEntry(Type messageType)
        {
            Contract.Requires(messageType != null);

            throw new NotImplementedException();
        }

        public void Add(IMessageEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(GetEntry(entry.MessageType) != null);
        }
    }
}
