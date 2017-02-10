using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionCatalog : IProjectionCatalog
    {
        private readonly List<IProjectionEntry> _entries = new List<IProjectionEntry>();
        
        public IReadOnlyCollection<IProjectionEntry> Entries
        {
            get { return _entries.AsReadOnly(); }
        }

        public ProjectionCatalog()
        {
        }

        public ProjectionCatalog(IReadOnlyCollection<IProjectionEntry> entries)
        {
            Contract.Requires(entries != null);
            Contract.Requires(Contract.ForAll(entries, o => o != null));

            foreach (var entry in entries)
            {
                Contract.Assume(entry != null);

                Add(entry);
            }
        }

        public IProjectionCatalog Add(IProjectionEntry entry)
        {
            if (_entries.Any(x => x.ProjectionType == entry.ProjectionType))
            {
                throw new ApplicationException("An item with the same projection type has already been added to the catalog.");
            }

            _entries.Add(entry);
            Contract.Assume(GetEntry(entry.ProjectionType) == entry);

            return this;
        }

        public IProjectionEntry GetEntry(ProjectionType projectionType)
        {
            return _entries.FirstOrDefault(x => x.ProjectionType == projectionType);
        }

        public ICollection<IProjectionEntry> GetEntries(AggregateRootType aggregateRootType)
        {
            return _entries.Where(x => x.AggregateRootType == aggregateRootType).ToList();
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
