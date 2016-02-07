using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Query
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly Dictionary<Type, IQueryHandler> _handlers;

        public IQueryCatalog Catalog { get; }

        public QueryHandlerFactory(IQueryCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;

            _handlers = new Dictionary<Type, IQueryHandler>();
        }

        public IQueryHandler GetHandler(object query)
        {
            var entry = GetEntry(query);

            return GetHandler(entry.QueryHandlerType);
        }

        protected virtual IQueryHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            return (IQueryHandler)Activator.CreateInstance(handlerType);
        }

        private IQueryHandler GetHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            // lock cache for concurrency
            lock (_handlers)
            {
                // find handler in cache
                var handler = GetCachedHandler(handlerType);

                // if handler is not cached, create and cache
                if (handler == null)
                {
                    handler = CreateHandler(handlerType);

                    CacheHandler(handler);
                }

                return handler;
            }
        }

        private void CacheHandler(IQueryHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Ensures((!handler.Reusable && _handlers[handler.GetType()] == null) ||
                             (handler.Reusable && _handlers[handler.GetType()] != null));

            // if handler is reusable, cache
            if (handler.Reusable)
            {
                _handlers[handler.GetType()] = handler;
            }
            else
            {
                Contract.Assume(_handlers[handler.GetType()] == null);
            }
        }

        private IQueryHandler GetCachedHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            IQueryHandler handler;

            _handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        private IQueryEntry GetEntry(object query)
        {
            Contract.Requires(query != null);
            Contract.Ensures(Contract.Result<IQueryEntry>() != null);

            var entry = Catalog.GetEntry(query.GetType());
            Contract.Assume(entry != null);

            return entry;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlers != null);
        }
    }
}
