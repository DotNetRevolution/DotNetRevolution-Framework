using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryHandlerFactory))]
    public abstract class QueryHandlerFactoryContract : IQueryHandlerFactory
    {
        public IQueryCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IQueryCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IQueryHandler GetHandler(object query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
