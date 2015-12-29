using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryCatalog))]
    public abstract class QueryCatalogContract : IQueryCatalog
    {
        public IQueryEntry this[Type queryType]
        {
            get 
            {
                Contract.Requires(queryType != null);
                Contract.Ensures(Contract.Result<IQueryEntry>() != null);

                throw new NotImplementedException();
            }
        }

        public void Add(IQueryEntry entry)
        {
            Contract.Requires(entry != null);
        }
    }
}