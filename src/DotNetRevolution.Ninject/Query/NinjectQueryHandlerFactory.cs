using System;
using DotNetRevolution.Core.Querying;
using Ninject;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Ninject.Query
{
    public class NinjectQueryHandlerFactory : QueryHandlerFactory
    {
        private readonly IKernel _kernel;

        public NinjectQueryHandlerFactory(IQueryCatalog catalog,
                                          IKernel kernel) 
            : base(catalog)
        {
            Contract.Requires(catalog != null);
            Contract.Requires(kernel != null);

            _kernel = kernel;
        }

        protected override IQueryHandler CreateHandler(Type handlerType)
        {
            return (IQueryHandler)_kernel.Get(handlerType) ?? base.CreateHandler(handlerType);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_kernel != null);
        }
    }
}
