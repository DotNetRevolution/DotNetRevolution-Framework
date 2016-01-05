using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageCatalog : IMessageCatalog
    {
        private readonly Dictionary<Type, IMessageEntry> _entries;

        public IMessageEntry this[Type messageType]
        {
            get
            {
                var result = _entries[messageType];
                Contract.Assume(result != null);

                return result;
            }
        }

        public MessageCatalog()
        {
            _entries = new Dictionary<Type, IMessageEntry>();
        }

        public void Add(IMessageEntry entry)
        {
            _entries.Add(entry.MessageType, entry);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
