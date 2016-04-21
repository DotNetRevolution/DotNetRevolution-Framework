using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class ShuttleMessageCatalog : IShuttleMessageCatalog
    {
        private readonly Dictionary<Type, IShuttleMessageEntry> _entries = new Dictionary<Type, IShuttleMessageEntry>();

        public IReadOnlyCollection<IShuttleMessageEntry> Entries
        {
            get { return _entries.Values.ToList().AsReadOnly(); }
        }

        public IShuttleMessageEntry this[Type messageType]
        {
            get
            {
                var result = _entries[messageType];
                Contract.Assume(result != null);

                return result;
            }
        }
        
        public void Add(IShuttleMessageEntry entry)
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
