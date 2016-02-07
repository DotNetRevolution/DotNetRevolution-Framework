using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers;

        public ICommandCatalog Catalog { get; }

        public CommandHandlerFactory(ICommandCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;

            _handlers = new Dictionary<Type, ICommandHandler>();
        }

        public ICommandHandler GetHandler(object command)
        {
            var entry = GetEntry(command);

            return GetHandler(entry.CommandHandlerType);
        }

        protected virtual ICommandHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

            return (ICommandHandler)Activator.CreateInstance(handlerType);
        }

        private ICommandHandler GetHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<ICommandHandler>() != null);

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

        private void CacheHandler(ICommandHandler handler)
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

        private ICommandHandler GetCachedHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            ICommandHandler handler;

            _handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        private ICommandEntry GetEntry(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandEntry>() != null);

            var entry = Catalog.GetEntry(command.GetType());
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
