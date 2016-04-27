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

    [ContractClassFor(typeof(IQueryHandler<,>))]
    internal abstract class QueryHandlerContract<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult> 
        where TResult : class
    {
        public abstract bool Reusable { get; }

        public TResult Handle(TQuery query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<TResult>() != null);

            throw new NotImplementedException();
        }

        public TResult1 Handle<TResult1>(IQuery<TResult1> query) where TResult1 : class
        {
            throw new NotImplementedException();
        }        
    }
}
