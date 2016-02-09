using DotNetRevolution.Core.Messaging;
using Ninject;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.Ninject.Message
{
    public class NinjectMessageHandlerFactory : MessageHandlerFactory
    {
        private readonly IKernel _kernel;

        public NinjectMessageHandlerFactory(IMessageCatalog catalog,
                                            IKernel kernel) 
            : base(catalog)
        {
            Contract.Requires(catalog != null);
            Contract.Requires(kernel != null);

            _kernel = kernel;
        }

        protected override IMessageHandler CreateHandler(Type handlerType)
        {
            return (IMessageHandler)_kernel.Get(handlerType) ?? base.CreateHandler(handlerType);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_kernel != null);
        }
    }
}
