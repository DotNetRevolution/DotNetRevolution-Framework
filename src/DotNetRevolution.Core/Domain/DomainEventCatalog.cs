using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventCatalog : IDomainEventCatalog
    {
        private readonly Dictionary<Type, List<IDomainEventEntry>> _entries = new Dictionary<Type, List<IDomainEventEntry>>();        

        public IReadOnlyCollection<Type> DomainEventTypes
        {
            get { return _entries.Keys.ToList().AsReadOnly(); }
        }

        public DomainEventCatalog()
        {
        }

        public DomainEventCatalog(IReadOnlyCollection<IDomainEventEntry> entries)
        {
            Contract.Requires(entries != null);
            Contract.Requires(Contract.ForAll(entries, o => o != null));

            foreach (var entry in entries)
            {
                Contract.Assume(entry != null);

                Add(entry);
            }
        }
        
        public void Add(IDomainEventEntry entry)
        {
            List<IDomainEventEntry> domainEventEntries;

            if (_entries.TryGetValue(entry.DomainEventType, out domainEventEntries))
            {
                Contract.Assume(domainEventEntries != null);

                domainEventEntries.Add(entry);
            }
            else
            {
                _entries.Add(entry.DomainEventType, new List<IDomainEventEntry> { entry });
            }
        }

        public IReadOnlyCollection<IDomainEventEntry> GetEntries(Type domainEventType)
        {            
            var result = _entries[domainEventType];
            Contract.Assume(result != null);

            return result.AsReadOnly();
        }

        public void Remove(IDomainEventEntry entry)
        {
            List<IDomainEventEntry> entries;

            if (_entries.TryGetValue(entry.DomainEventType, out entries))
            {
                Contract.Assume(entries != null);

                entries.Remove(entry);

                Contract.Assume(_entries.TryGetValue(entry.DomainEventType, out entries) == false ||
                    (_entries.TryGetValue(entry.DomainEventType, out entries) && !entries.Contains(entry)));
            }
        }

        public bool TryGetEntries(Type domainEventType, out IReadOnlyCollection<IDomainEventEntry> entries)
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
        
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
