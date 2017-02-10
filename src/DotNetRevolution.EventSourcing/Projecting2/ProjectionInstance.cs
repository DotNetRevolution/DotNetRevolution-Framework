using System.Diagnostics.Contracts;
namespace DotNetRevolution.EventSourcing.Projecting2
{
    public class ProjectionInstance
    {
        public ProjectionType ProjectionType { get; }

        public EventProviderIdentity EventProviderIdentity { get; }

        public ProjectionPosition Position { get; }

        public ProjectionInstance(ProjectionType projectionType, EventProviderIdentity eventProviderIdentity, ProjectionPosition position)
        {
            Contract.Requires(projectionType != null);
            Contract.Requires(eventProviderIdentity != null);
            Contract.Requires(position != null);

            ProjectionType = projectionType;
            EventProviderIdentity = eventProviderIdentity;
            Position = position;
        }
    }
}
