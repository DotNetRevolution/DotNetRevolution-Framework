using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying.CodeContract
{
    [ContractClassFor(typeof(IQueryHandler))]
    internal abstract class QueryHandlerContract : IQueryHandler
    {
        public abstract bool Reusable { get; }

        public TResult Handle<TResult>(IQuery<TResult> query) where TResult : class
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<object>() != null);

            throw new NotImplementedException();
        }
    }
}
