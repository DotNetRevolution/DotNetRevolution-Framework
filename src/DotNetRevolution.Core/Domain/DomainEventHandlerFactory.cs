using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerFactory : IDomainEventHandlerFactory
    {
        private readonly IDictionary<Type, IDictionary<Type, IDomainEventHandler>> _handlers = new Dictionary<Type, IDictionary<Type, IDomainEventHandler>>();

        public IDomainEventCatalog Catalog { get; }

        public DomainEventHandlerFactory(IDomainEventCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;
        }

        public IDomainEventHandlerCollection GetHandlers(Type domainEventType)
        {
            // get entries from catalog
            var entries = GetEntries(domainEventType);

            return new DomainEventHandlerCollection(domainEventType, GetHandlers(domainEventType, entries));
        }

        private IReadOnlyCollection<IDomainEventHandler> GetHandlers(Type domainEventType, IReadOnlyCollection<IDomainEventEntry> entries)
        {
            Contract.Requires(domainEventType != null);
            Contract.Requires(entries != null);
            Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEventHandler>>() != null);

            var results = new Collection<IDomainEventHandler>();

            // lock cache for concurrency
            lock (_handlers)
            {
                foreach (var entry in entries)
                {
                    Contract.Assume(entry != null);

                    IDictionary<Type, IDomainEventHandler> cachedHandlers = GetCachedHandlers(domainEventType);

                    // add handlers to results
                    results.Add(GetHandler(entry.DomainEventHandlerType, cachedHandlers));
                }
            }

            return results;
        }

        private IDictionary<Type, IDomainEventHandler> GetCachedHandlers(Type domainEventType)
        {
            Contract.Requires(domainEventType != null);
            Contract.Ensures(Contract.Result<IDictionary<Type, IDomainEventHandler>>() != null);

            IDictionary<Type, IDomainEventHandler> cachedHandlers;

            // try to get the cached dictionary of handlers for entry
            if (_handlers.TryGetValue(domainEventType, out cachedHandlers))
            {
                Contract.Assume(cachedHandlers != null);
                return cachedHandlers;
            }

            return _handlers[domainEventType] = new Dictionary<Type, IDomainEventHandler>();
        }

        protected virtual IDomainEventHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<IDomainEventHandler>() != null);

            return (IDomainEventHandler)Activator.CreateInstance(handlerType);
        }

        private IDomainEventHandler GetHandler(Type handlerType, IDictionary<Type, IDomainEventHandler> handlers)
        {
            Contract.Requires(handlerType != null);
            Contract.Requires(handlers != null);
            Contract.Ensures(Contract.Result<IDomainEventHandler>() != null);

            var handler = GetCachedHandler(handlerType, handlers);

            if (handler == null)
            {
                handler = CreateHandler(handlerType);

                CacheHandler(handler, handlers);
            }

            return handler;
        }

        private static void CacheHandler(IDomainEventHandler handler, IDictionary<Type, IDomainEventHandler> handlers)
        {
            Contract.Requires(handler != null);
            Contract.Requires(handlers != null);
            Contract.Ensures((handler.Reusable == false && GetCachedHandler(handler.GetType(), handlers) == null) ||
                             (handler.Reusable && handlers[handler.GetType()] != null));

            // if handler is reusable, cache
            if (handler.Reusable)
            {
                handlers[handler.GetType()] = handler;
            }
            else
            {
                Contract.Assume(GetCachedHandler(handler.GetType(), handlers) == null);
            }
        }

        [Pure]
        private static IDomainEventHandler GetCachedHandler(Type handlerType, IDictionary<Type, IDomainEventHandler> handlers)
        {
            Contract.Requires(handlerType != null);
            Contract.Requires(handlers != null);

            IDomainEventHandler handler = null;

            handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        [Pure]
        private IReadOnlyCollection<IDomainEventEntry> GetEntries(Type domainEventType)
        {
            Contract.Requires(domainEventType != null);
            Contract.Ensures(Contract.Result<IReadOnlyCollection<IDomainEventEntry>>() != null);
            
            // get type of domain event
            var results = new List<IDomainEventEntry>();

            // find entries in catalog based on domain event type
            do
            {
                IReadOnlyCollection<IDomainEventEntry> entries;

                if (Catalog.TryGetEntries(domainEventType, out entries) == false)
                {
                    // no entries, try base type if any
                    continue;
                }

                Contract.Assume(entries != null);
                results.AddRange(entries);

                // while base type is not null, attempt to find entries.  this is for backwards compatibility
            } while ((domainEventType = domainEventType.BaseType) != null);

            Contract.Assume(Contract.ForAll(results, o => o != null));

            return results;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlers != null);
        }
    }
}
