using DotNetRevolution.Core.Query.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    [ContractClass(typeof(QueryHandlerFactoryContract))]
    public interface IQueryHandlerFactory
    {
        IQueryCatalog Catalog { [Pure] get; }

        IQueryHandler GetHandler(object query);
    }
}
