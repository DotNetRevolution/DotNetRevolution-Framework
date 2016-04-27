using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying.CodeContract
{
    [ContractClassFor(typeof(IQueryDispatcher))]
    internal abstract class QueryDispatcherContract : IQueryDispatcher
    {
        public void UseCatalog(IQueryCatalog catalog)
        {
            Contract.Requires(catalog != null);
        }

        public TResult Dispatch<TResult>(IQuery<TResult> query) where TResult : class
        {
            Contract.Requires(query != null);

            return default(TResult);
        }
    }
}
