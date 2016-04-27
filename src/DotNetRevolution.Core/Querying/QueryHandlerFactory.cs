using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly Dictionary<Type, IQueryHandler> _handlers = new Dictionary<Type, IQueryHandler>();

        public IQueryCatalog Catalog { get; }

        public QueryHandlerFactory(IQueryCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;
        }

        public IQueryHandler GetHandler(Type queryType)
        {
            var entry = GetEntry(queryType);

            var handlerType = entry.QueryHandlerType;

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

        protected virtual IQueryHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<IQueryHandler>() != null);

            return (IQueryHandler)Activator.CreateInstance(handlerType);
        }
        
        private void CacheHandler(IQueryHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Ensures((!handler.Reusable && GetCachedHandler(handler.GetType()) == null) ||
                             (handler.Reusable && _handlers[handler.GetType()] != null));

            // if handler is reusable, cache
            if (handler.Reusable)
            {
                _handlers[handler.GetType()] = handler;
            }
            else
            {
                Contract.Assume(GetCachedHandler(handler.GetType()) == null);
            }
        }

        [Pure]
        private IQueryHandler GetCachedHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            IQueryHandler handler;

            _handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        [Pure]
        private IQueryEntry GetEntry(Type queryType)
        {
            Contract.Requires(queryType != null);
            Contract.Ensures(Contract.Result<IQueryEntry>() != null);

            var entry = Catalog.GetEntry(queryType);
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
