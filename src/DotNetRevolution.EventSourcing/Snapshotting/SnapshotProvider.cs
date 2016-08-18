using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public abstract class SnapshotProvider<TAggregateRoot> : ISnapshotProvider<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public abstract Snapshot GetSnapshot(TAggregateRoot aggregateRoot);

        public Snapshot CreateSnapshot(IAggregateRoot aggregateRoot)
        {
            return GetSnapshot((TAggregateRoot) aggregateRoot);
        }        
    }
}
