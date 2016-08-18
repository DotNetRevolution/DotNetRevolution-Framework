namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class NoSnapshotPolicy : ISnapshotPolicy
    {
        public bool Check(IEventStream eventStream)
        {
            return false;
        }
    }
}
