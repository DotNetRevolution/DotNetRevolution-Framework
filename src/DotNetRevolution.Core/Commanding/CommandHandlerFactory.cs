using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers = new Dictionary<Type, ICommandHandler>();

        public ICommandCatalog Catalog { get; }

        public CommandHandlerFactory(ICommandCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;
        }

        public ICommandHandler GetHandler(Type commandType)
        {
            var entry = GetEntry(commandType);

            var handlerType = entry.CommandHandlerType;

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
        
        [Pure]
        protected virtual ICommandHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            return (ICommandHandler)Activator.CreateInstance(handlerType);
        }
        
        private void CacheHandler(ICommandHandler handler)
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
        private ICommandHandler GetCachedHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            ICommandHandler handler;

            _handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        [Pure]
        private ICommandEntry GetEntry(Type commandType)
        {
            Contract.Requires(commandType != null);
            Contract.Ensures(Contract.Result<ICommandEntry>() != null);

            var entry = Catalog.GetEntry(commandType);
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
