using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Projecting
{
    public class ProjectionCatalog : IProjectionCatalog
    {
        private readonly Dictionary<Type, IProjectionEntry> _entries = new Dictionary<Type, IProjectionEntry>();
        
        public IReadOnlyCollection<IProjectionEntry> Entries
        {
            get { return _entries.Values.ToList().AsReadOnly(); }
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
            _entries.Add(entry.ProjectionType, entry);

            Contract.Assume(GetEntry(entry.ProjectionType) == entry);

            return this;
        }

        public IProjectionEntry GetEntry(Type projectionType)
        {
            var result = _entries[projectionType];
            Contract.Assume(result != null);

            return result;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
