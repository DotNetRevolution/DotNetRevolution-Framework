using DotNetRevolution.Core.Command;
using System.Diagnostics.Contracts;
using System;
using Ninject;

namespace DotNetRevolution.Ninject.Command
{
    public class NinjectCommandHandlerFactory : CommandHandlerFactory
    {
        private readonly IKernel _kernel;

        public NinjectCommandHandlerFactory(ICommandCatalog catalog,
                                            IKernel kernel) 
            : base(catalog)
        {
            Contract.Requires(catalog != null);
            Contract.Requires(kernel != null);

            _kernel = kernel;            
        }

        protected override ICommandHandler CreateHandler(Type handlerType)
        {
            return (ICommandHandler) _kernel.Get(handlerType) ?? base.CreateHandler(handlerType);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_kernel != null);
        }
    }
}
