using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryCatalog : IQueryCatalog
    {
        protected Dictionary<Type, IQueryEntry> Entries { get; private set; }
        
        public IQueryEntry this[Type queryType]
        {
            get
            {
                var result = Entries[queryType];
                Contract.Assume(result != null);

                return result;
            }
        }

        public QueryCatalog()
        {
            Entries = new Dictionary<Type, IQueryEntry>();
        }

        public void Add(IQueryEntry entry)
        {
            Entries.Add(entry.QueryType, entry);

            Contract.Assume(this[entry.QueryType] != null);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(Entries != null);
        }
    }
}
