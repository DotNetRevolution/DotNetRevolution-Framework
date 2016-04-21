using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageCatalog : IMessageCatalog
    {
        private readonly Dictionary<Type, IMessageEntry> _entries = new Dictionary<Type, IMessageEntry>();

        public MessageCatalog()
        {
        }

        public MessageCatalog(IReadOnlyCollection<IMessageEntry> entries)
        {
            Contract.Requires(entries != null);
            Contract.Requires(Contract.ForAll(entries, o => o != null));

            foreach (var entry in entries)
            {
                Add(entry);
            }
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

            Contract.Assume(GetEntry(entry.MessageType) == entry);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
