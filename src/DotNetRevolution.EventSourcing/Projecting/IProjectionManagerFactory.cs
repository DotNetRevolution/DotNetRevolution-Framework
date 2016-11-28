using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionManagerFactoryContract))]
    public interface IProjectionManagerFactory
    {
        IProjectionCatalog Catalog { get; }

        IProjectionManager GetManager(Type projectionType);

        IEnumerable<IProjectionManager> GetManagers();
    }
}