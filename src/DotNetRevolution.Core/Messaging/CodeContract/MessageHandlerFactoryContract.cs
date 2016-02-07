using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageHandlerFactory))]
    public abstract class MessageHandlerFactoryContract : IMessageHandlerFactory
    {
        public IMessageCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IMessageCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IMessageHandler GetHandler(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<IMessageHandler>() != null);

            throw new NotImplementedException();
        }
    }
}
