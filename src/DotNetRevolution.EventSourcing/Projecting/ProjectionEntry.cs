using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionEntry : IProjectionEntry
    {
        public AggregateRootType AggregateRootType { get; }

        public IProjectionManager ProjectionManager { get; }

        public ProjectionType ProjectionType { get; }

        public Type ProjectionManagerType { get; }

        public ProjectionEntry(AggregateRootType aggregateRootType, ProjectionType projectionType, Type projectionManagerType)
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Requires(projectionType != null);
            Contract.Requires(projectionManagerType != null);

            AggregateRootType = aggregateRootType;
            ProjectionType = projectionType;
            ProjectionManagerType = projectionManagerType;
        }

        public ProjectionEntry(AggregateRootType aggregateRootType, ProjectionType projectionType, IProjectionManager projectionManager)
            : this(aggregateRootType, projectionType, projectionManager.GetType())
        {
            Contract.Requires(aggregateRootType != null);
            Contract.Requires(projectionType != null);
            Contract.Requires(projectionManager != null);

            ProjectionManager = projectionManager;
        }        
    }
}