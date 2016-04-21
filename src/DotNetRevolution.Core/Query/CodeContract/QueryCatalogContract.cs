using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryCatalog))]
    internal abstract class QueryCatalogContract : IQueryCatalog
    {
        public IQueryEntry GetEntry(Type queryType)
        {
            Contract.Requires(queryType != null);

            throw new NotImplementedException();
        }

        public IQueryCatalog Add(IQueryEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(Contract.Result<IQueryCatalog>() != null);
            Contract.Ensures(GetEntry(entry.QueryType) == entry);
            Contract.EnsuresOnThrow<ArgumentException>(GetEntry(entry.QueryType) == Contract.OldValue(GetEntry(entry.QueryType)));

            throw new NotImplementedException();
        }
    }
}
