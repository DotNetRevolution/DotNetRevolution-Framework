using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Querying.CodeContract;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryCatalogContract))]
    public interface IQueryCatalog
    {
        [Pure]
        IQueryEntry GetEntry(Type queryType);

        IQueryCatalog Add(IQueryEntry entry);
    }
}
