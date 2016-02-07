using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageCatalog : IMessageCatalog
    {
        private readonly Dictionary<Type, IMessageEntry> _entries;
        
        public MessageCatalog()
        {
            _entries = new Dictionary<Type, IMessageEntry>();
        }

        public IMessageEntry GetEntry(Type messageType)
        {
            var result = _entries[messageType];
            Contract.Assume(result != null);

            return result;
        }

        public void Add(IMessageEntry entry)
        {
            _entries.Add(entry.MessageType, entry);

            Contract.Assume(GetEntry(entry.MessageType) != null);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
