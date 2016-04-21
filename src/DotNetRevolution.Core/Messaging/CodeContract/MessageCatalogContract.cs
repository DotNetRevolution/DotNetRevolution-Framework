using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageCatalog))]
    internal abstract class MessageCatalogContract : IMessageCatalog
    {
        public void Add(IMessageEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(GetEntry(entry.MessageType) == entry);
            Contract.EnsuresOnThrow<ArgumentException>(GetEntry(entry.MessageType) == Contract.OldValue(GetEntry(entry.MessageType)));
        }

        public IMessageEntry GetEntry(Type messageType)
        {
            Contract.Requires(messageType != null);

            throw new NotImplementedException();
        }
    }
}
