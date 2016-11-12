using DotNetRevolution.Core.Projecting.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    [ContractClass(typeof(ProjectionManagerFactoryContract))]
    public interface IProjectionManagerFactory
    {
        IProjectionCatalog Catalog { get; }

        IProjectionManager GetManager(Type projectionType);

        IEnumerable<IProjectionManager> GetManagers();
    }
}