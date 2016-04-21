using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageHandlerFactory))]
    internal abstract class MessageHandlerFactoryContract : IMessageHandlerFactory
    {
        public IMessageCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IMessageCatalog>() != null);

                throw new NotImplementedException();
            }
        }
        
        public IMessageHandler GetHandler(Type messageType)
        {
            Contract.Requires(messageType != null);
            Contract.Ensures(Contract.Result<IMessageHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
