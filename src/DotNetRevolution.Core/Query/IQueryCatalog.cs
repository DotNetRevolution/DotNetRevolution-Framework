using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Query.CodeContract;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryCatalogContract))]
    public interface IQueryCatalog
    {
        IQueryEntry this[Type queryType] { [Pure] get; }

        void Add(IQueryEntry entry);
    }
}
