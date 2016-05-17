using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IMessageHandlerFactory _handlerFactory;

        public MessageDispatcher(IMessageHandlerFactory handlerFactory)
        {
            Contract.Requires(handlerFactory != null);

            _handlerFactory = handlerFactory;
        }
        
        public void Dispatch(IMessage message)
        {
            var correlationId = Guid.NewGuid().ToString();            
            Contract.Assume(string.IsNullOrWhiteSpace(correlationId) == false);

            Dispatch(message, correlationId);
        }

        public void Dispatch(IMessage message, string correlationId)
        {
            IMessageHandler handler = GetHandler(message);
            HandleMessage(message, handler, correlationId);
        }

        private IMessageHandler GetHandler(IMessage message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<IMessageHandler>() != null);

            try
            {
                // get handler from factory
                return _handlerFactory.GetHandler(message.GetType());
            }
            catch (Exception e)
            {
                // rethrow exception has a message handling exception
                throw new MessageHandlingException(message, e, "Could not get a message handler for message.");
            }
        }

        private static void HandleMessage(IMessage message, IMessageHandler handler, string correlationId)
        {
            Contract.Requires(handler != null);
            Contract.Requires(message != null);

            try
            {
                // handle message
                handler.Handle(message, correlationId);
            }
            catch (Exception e)
            {
                // re-throw exception as a message handling exception
                throw new MessageHandlingException(message, e, "Exception occurred in message handler, check inner exception for details.");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_handlerFactory != null);
        }
    }
}
