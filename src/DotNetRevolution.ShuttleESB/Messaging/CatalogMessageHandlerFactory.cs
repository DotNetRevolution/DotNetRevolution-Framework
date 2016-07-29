using System;
using System.Diagnostics.Contracts;
using Shuttle.Esb;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class CatalogMessageHandlerFactory : DefaultMessageHandlerFactory
    {
        private readonly IShuttleMessageCatalog _catalog;

        public CatalogMessageHandlerFactory(IShuttleMessageCatalog catalog)
        {
            Contract.Requires(catalog != null);

            _catalog = catalog;
        }

        public override IMessageHandler CreateHandler(object message)
        {
            Contract.Assume(message != null);

            // get entry
            var entry = _catalog[message.GetType()];

            // get a  handler
            return GetHandler(entry);
        }

        protected virtual IMessageHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            return (IMessageHandler) Activator.CreateInstance(handlerType);
        }

        private IMessageHandler GetHandler(IShuttleMessageEntry entry)
        {
            Contract.Requires(entry != null);

            // get handler from entry
            var handler = entry.MessageHandler;

            // if handler is cached, return handler
            if (handler != null)
            {
                return handler;
            }

            // create handler
            handler = CreateHandler(entry.MessageHandlerType);
            Contract.Assume(handler != null);

            // if handler is reusable, cache in entry
            if (handler.IsReusable)
            {
                entry.MessageHandler = handler;
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
