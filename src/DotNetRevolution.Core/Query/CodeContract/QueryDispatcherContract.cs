using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryDispatcher))]
    public abstract class QueryDispatcherContract : IQueryDispatcher
    {
        public void UseCatalog(IQueryCatalog catalog)
        {
            Contract.Requires(catalog != null);
        }

        public TResult Dispatch<TResult>(object query)
        {
            Contract.Requires(query != null);

            return default(TResult);
        }
    }
}
