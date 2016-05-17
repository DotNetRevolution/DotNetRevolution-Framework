using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly Dictionary<Type, IMessageHandler> _handlers = new Dictionary<Type, IMessageHandler>();

        public IMessageCatalog Catalog { get; }

        public MessageHandlerFactory(IMessageCatalog catalog)
        {
            Contract.Requires(catalog != null);

            Catalog = catalog;
        }

        public IMessageHandler GetHandler(Type messageType)
        {
            var entry = GetEntry(messageType);

            var handlerType = entry.MessageHandlerType;

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

        protected virtual IMessageHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);
            Contract.Ensures(Contract.Result<IMessageHandler>() != null);

            return (IMessageHandler)Activator.CreateInstance(handlerType);
        }
        
        private void CacheHandler(IMessageHandler handler)
        {
            Contract.Requires(handler != null);
            Contract.Ensures((handler.Reusable && _handlers[handler.GetType()] != null) ||
                             (handler.Reusable == false && GetCachedHandler(handler.GetType()) == null));

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
        private IMessageHandler GetCachedHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            IMessageHandler handler;

            _handlers.TryGetValue(handlerType, out handler);

            return handler;
        }

        [Pure]
        private IMessageEntry GetEntry(Type messageType)
        {
            Contract.Requires(messageType != null);
            Contract.Ensures(Contract.Result<IMessageEntry>() != null);

            var entry = Catalog.GetEntry(messageType);
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
