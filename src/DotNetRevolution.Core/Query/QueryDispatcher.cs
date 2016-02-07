using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryHandlerFactory _handlerFactory;

        public QueryDispatcher(IQueryHandlerFactory handlerFactory)
        {
            Contract.Requires(handlerFactory != null);

            _handlerFactory = handlerFactory;
        }

        public TResult Dispatch<TResult>(object query) where TResult : class
        {
            IQueryHandler handler = GetHandler(query);
            return HandleQuery<TResult>(query, handler);
        }
        
        private IQueryHandler GetHandler(object query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            try
            {
                // get handler from factory
                return _handlerFactory.GetHandler(query.GetType());
            }
            catch (Exception e)
            {
                // rethrow exception has a query handling exception
                throw new QueryHandlingException(query, e, "Could not get a query handler for query.");
            }
        }

        private static TResult HandleQuery<TResult>(object query, IQueryHandler handler)
            where TResult : class
        {
            Contract.Requires(handler != null);
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<TResult>() != null);

            try
            {
                // handle query
                return (TResult) handler.Handle(query);
            }
            catch (Exception e)
            {
                // re-throw exception as a query handling exception
                throw new QueryHandlingException(query, e, "Exception occurred in query handler, check inner exception for details.");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
        }
    }
}
