using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
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

        public IProjectionManager GetManager(ProjectionType projectionType)
        {
            Contract.Requires(projectionType != null);

            throw new NotImplementedException();
        }

        public ICollection<IProjectionManager> GetManagers(AggregateRootType aggregateRootType)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<IEnumerable<IProjectionManager>>() != null);

            throw new NotImplementedException();
        }
    }
}
