using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionManagerFactory))]
    internal abstract class ProjectionManagerFactoryContract : IProjectionManagerFactory
    {
        public IProjectionCatalog Catalog
        {
            get
            {
                Contract.Ensures(Contract.Result<IProjectionCatalog>() != null);

                throw new NotImplementedException();
            }
        }

        public IProjectionManager GetManager(Type projectionType)
        {
            Contract.Requires(projectionType != null);
            Contract.Ensures(Contract.Result<IProjectionManager>() != null);

            throw new NotImplementedException();
        }

        public IEnumerable<IProjectionManager> GetManagers()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IProjectionManager>>() != null);

            throw new NotImplementedException();
        }
    }
}
