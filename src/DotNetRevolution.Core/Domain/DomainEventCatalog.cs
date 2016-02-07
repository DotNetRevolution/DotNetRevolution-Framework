using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventCatalog : IDomainEventCatalog
    { 
        private readonly Dictionary<Type, List<IDomainEventEntry>> _entries;

        public IReadOnlyCollection<Type> DomainEventTypes
        {
            get { return _entries.Keys.ToList().AsReadOnly(); }
        }
        
        public DomainEventCatalog()
        {
            _entries = new Dictionary<Type, List<IDomainEventEntry>>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public IDomainEventEntryRegistration Add(IDomainEventEntry entry)
        {
            lock (_entries)
            {
                AddEntry(entry);

                return new DomainEventEntryRegistration(this, entry);
            }
        }

        public IReadOnlyCollection<IDomainEventEntry> GetEntries(Type domainEventType)
        {
            lock (_entries)
            {
                var result = _entries[domainEventType];
                Contract.Assume(result != null);

                return result.AsReadOnly();
            }
        }

        public void Remove(IDomainEventEntry entry)
        {
            lock (_entries)
            {
                List<IDomainEventEntry> entries;

                if (_entries.TryGetValue(entry.DomainEventType, out entries))
                {
                    Contract.Assume(entries != null);

                    entries.Remove(entry);
                }
            }
        }

        public bool TryGetEntries(Type domainEventType, out IReadOnlyCollection<IDomainEventEntry> entries)
        {
            lock (_entries)
            {
                List<IDomainEventEntry> eventEntries;

                if (_entries.TryGetValue(domainEventType, out eventEntries))
                {
                    Contract.Assume(eventEntries != null);
                    Contract.Assume(Contract.ForAll(eventEntries, entry => entry != null));

                    entries = eventEntries.AsReadOnly();
                    return true;
                }

                entries = null;
                return false;
            }
        }

        private void AddEntry(IDomainEventEntry entry)
        {
            Contract.Requires(entry != null);

            List<IDomainEventEntry> domainEventEntries;

            if (_entries.TryGetValue(entry.DomainEventType, out domainEventEntries))
            {
                Contract.Assume(domainEventEntries != null);
                Contract.Assume(Contract.ForAll(domainEventEntries, eventEntry => eventEntry != null));

                domainEventEntries.Add(entry);
                return;
            }

            _entries.Add(entry.DomainEventType, new List<IDomainEventEntry> { entry });
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
