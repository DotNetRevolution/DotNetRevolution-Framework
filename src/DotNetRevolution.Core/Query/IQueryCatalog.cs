using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryCatalogContract))]
    public interface IQueryCatalog
    {
        [Pure]
        IQueryEntry this[Type queryType] { get; }

        void Add(IQueryEntry entry);
    }
}
