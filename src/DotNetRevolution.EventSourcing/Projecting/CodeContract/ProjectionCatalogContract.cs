using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionCatalog))]
    internal abstract class ProjectionCatalogContract : IProjectionCatalog
    {
        public IReadOnlyCollection<IProjectionEntry> Entries
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IProjectionEntry>>() != null);

                throw new NotImplementedException();
            }
        }

        public IProjectionCatalog Add(IProjectionEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(Contract.Result<IProjectionCatalog>() != null);
            Contract.Ensures(GetEntry(entry.ProjectionType) == entry);
            Contract.EnsuresOnThrow<ArgumentException>(GetEntry(entry.ProjectionType) == Contract.OldValue(GetEntry(entry.ProjectionType)));

            throw new NotImplementedException();
        }

        public ICollection<IProjectionEntry> GetEntries(AggregateRootType aggregateRootType)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Ensures(Contract.Result<ICollection<IProjectionEntry>>() != null);

            throw new NotImplementedException();
        }

        public IProjectionEntry GetEntry(ProjectionType projectionType)
        {
            Contract.Requires(projectionType != null);

            throw new NotImplementedException();
        }
    }
}
