using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class ProjectionEntry : IProjectionEntry
    {
        public IProjectionManager ProjectionManager { get; }

        public Type ProjectionType { get; }

        public Type ProjectionManagerType { get; }

        public ProjectionEntry(Type projectionType, Type projectionManagerType)
        {
            Contract.Requires(projectionType != null);
            Contract.Requires(projectionManagerType != null);

            ProjectionType = projectionType;
            ProjectionManagerType = projectionManagerType;
        }

        public ProjectionEntry(Type projectionType, IProjectionManager projectionManager)
            : this(projectionType, projectionManager.GetType())
        {
            Contract.Requires(projectionType != null);
            Contract.Requires(projectionManager != null);

            ProjectionManager = projectionManager;
        }        
    }
}