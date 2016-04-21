using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryCatalog : IQueryCatalog
    {
        private readonly Dictionary<Type, IQueryEntry> _entries = new Dictionary<Type, IQueryEntry>();
                
        public QueryCatalog()
        {
        }

        public QueryCatalog(IReadOnlyCollection<IQueryEntry> entries)
        {
            Contract.Requires(entries != null);
            Contract.Requires(Contract.ForAll(entries, o => o != null));

            foreach (var entry in entries)
            {
                Add(entry);
            }
        }
        public IQueryEntry GetEntry(Type queryType)
        {
            var result = _entries[queryType];
            Contract.Assume(result != null);

            return result;
        }

        public void Add(IQueryEntry entry)
        {
            _entries.Add(entry.QueryType, entry);

            Contract.Assume(GetEntry(entry.QueryType) == entry);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_entries != null);
        }
    }
}
