using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryHandlerFactory))]
    internal abstract class QueryHandlerFactoryContract : IQueryHandlerFactory
    {
        public IQueryCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IQueryCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IQueryHandler GetHandler(Type queryType)
        {
            Contract.Requires(queryType != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
