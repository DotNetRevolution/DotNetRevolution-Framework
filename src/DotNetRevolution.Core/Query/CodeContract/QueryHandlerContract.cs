using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query.CodeContract
{
    [ContractClassFor(typeof(IQueryHandler))]
    public abstract class QueryHandlerContract : IQueryHandler
    {
        public abstract bool Reusable { get; }

        public object Handle(object query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<object>() != null);

            throw new NotImplementedException();
        }
    }

    [ContractClassFor(typeof(IQueryHandler<,>))]
    public abstract class QueryHandlerContract<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class 
        where TResult : class
    {
        public abstract bool Reusable { get; }

        public TResult Handle(TQuery query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<TResult>() != null);

            throw new NotImplementedException();
        }

        public object Handle(object query)
        {
            throw new NotImplementedException();
        }
    }
}
