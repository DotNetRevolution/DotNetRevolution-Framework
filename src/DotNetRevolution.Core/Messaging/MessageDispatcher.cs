using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IMessageCatalog _catalog;

        public MessageDispatcher(IMessageCatalog catalog)
        {
            Contract.Requires(catalog != null);

            _catalog = catalog;
        }
        
        public void Dispatch(object message)
        {
            var correlationId = Guid.NewGuid().ToString();            
            Contract.Assume(!string.IsNullOrWhiteSpace(correlationId));

            Dispatch(message, correlationId);
        }

        public void Dispatch(object message, string correlationId)
        {
            try
            {
                // get entry
                var entry = _catalog[message.GetType()];
                
                // get a message handler
                var handler = GetHandler(entry);

                // handle message
                handler.Handle(message, correlationId);
            }
            catch (Exception e)
            {
                // re-throw exception as a message handling exception
                throw new MessageHandlingException(message, e, "Exception occurred in message handler, check inner exception for details.");
            }
        }

        protected virtual IMessageHandler CreateHandler(Type handlerType)
        {
            Contract.Requires(handlerType != null);

            return (IMessageHandler)Activator.CreateInstance(handlerType);
        }

        private IMessageHandler GetHandler(IMessageEntry entry)
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
            if (handler.Reusable)
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