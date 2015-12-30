using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryCatalog _catalog;

        public QueryDispatcher(IQueryCatalog queryCatalog)
        {
            Contract.Requires(queryCatalog != null);

            _catalog = queryCatalog;
        }

        public TResult Dispatch<TResult>(object query)
        {
            try
            {
                // get entry
                var entry = _catalog[query.GetType()];
                
                // get a query handler
                var handler = GetHandler(entry);

                // handle query
                return (TResult)handler.Handle(query);
            }
            catch (Exception e)
            {
                // re-throw exception as a query handling exception
                throw new QueryHandlingException(query, e, "Exception occurred in query handler, check inner exception for details.");
            }
        }

        protected virtual IQueryHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            return (IQueryHandler) Activator.CreateInstance(handlerType);
        }

        private IQueryHandler GetHandler(IQueryEntry entry)
        {
            Contract.Requires(entry != null);

            // get handler from entry
            var handler = entry.QueryHandler;

            // if handler is cached, return handler
            if (handler != null)
            {
                return handler;
            }

            // create handler
            handler = CreateHandler(entry.QueryHandlerType);
            Contract.Assume(handler != null);

            // if handler is reusable, cache in entry
            if (handler.Reusable)
            {
                entry.QueryHandler = handler;
            }

            return handler;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_catalog != null);
        }
    }
}