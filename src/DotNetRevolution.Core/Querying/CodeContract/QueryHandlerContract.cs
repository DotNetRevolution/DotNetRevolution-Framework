using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Querying.CodeContract
{
    [ContractClassFor(typeof(IQueryHandler))]
    internal abstract class QueryHandlerContract : IQueryHandler
    {
        public abstract bool Reusable { get; }

        public TResult Handle<TResult>(IQuery<TResult> query) where TResult : class
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<TResult>() != null);

            throw new NotImplementedException();
        }

        public Task<TResult> HandleAsync<TResult>(IQuery<TResult> query) where TResult : class
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<Task<TResult>>() != null);
            
            throw new NotImplementedException();
        }
    }
}
