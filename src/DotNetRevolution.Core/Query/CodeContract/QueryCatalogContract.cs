using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryCatalog))]
    internal abstract class QueryCatalogContract : IQueryCatalog
    {
        public IQueryEntry this[Type queryType]
        {
            get 
            {
                Contract.Requires(queryType != null);

                throw new NotImplementedException();
            }
        }

        public void Add(IQueryEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(this[entry.QueryType] != null);
        }
    }
}
