using DotNetRevolution.Core.Domain;
using Ninject;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.Ninject.Domain
{
    public class NinjectDomainEventHandlerFactory : DomainEventHandlerFactory
    {
        private readonly IKernel _kernel;

        public NinjectDomainEventHandlerFactory(IDomainEventCatalog catalog,
                                                IKernel kernel) 
            : base(catalog)
        {
            Contract.Requires(catalog != null);
            Contract.Requires(kernel != null);

            _kernel = kernel;
        }

        protected override IDomainEventHandler CreateHandler(Type handlerType)
        {
            return (IDomainEventHandler)_kernel.Get(handlerType) ?? base.CreateHandler(handlerType);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_kernel != null);
        }
    }
}
