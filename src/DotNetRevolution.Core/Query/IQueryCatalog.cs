using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryCatalogContract))]
    public interface IQueryCatalog
    {
        [Pure]
        IQueryEntry GetEntry(Type queryType);

        IQueryCatalog Add(IQueryEntry entry);
    }
}
