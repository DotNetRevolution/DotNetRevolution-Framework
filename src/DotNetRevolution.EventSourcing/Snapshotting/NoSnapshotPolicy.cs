namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class NoSnapshotPolicy : ISnapshotPolicy
    {
        public bool Check(IEventProvider eventProvider)
        {
            return false;
        }
    }
}
