using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

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
            Contract.Ensures(Contract.Result<TResult>() != null);
            
            return default(TResult);
        }

        public Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query) where TResult : class
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<Task<TResult>>() != null);

            throw new NotImplementedException();
        }
    }
}
