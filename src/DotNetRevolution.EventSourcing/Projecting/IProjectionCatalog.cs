using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionCatalogContract))]
    public interface IProjectionCatalog
    {
        IReadOnlyCollection<IProjectionEntry> Entries { get; }

        IProjectionCatalog Add(IProjectionEntry entry);

        [Pure]
        IProjectionEntry GetEntry(Type projectionType);
    }
}