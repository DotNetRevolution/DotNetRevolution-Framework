using DotNetRevolution.Core.Querying.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying
{
    [ContractClass(typeof(QueryHandlerFactoryContract))]
    public interface IQueryHandlerFactory
    {
        IQueryCatalog Catalog { [Pure] get; }

        IQueryHandler GetHandler(Type queryType);
    }
}
