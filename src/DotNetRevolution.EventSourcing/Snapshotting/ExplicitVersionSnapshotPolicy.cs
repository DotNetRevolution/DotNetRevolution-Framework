namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class ExplicitVersionSnapshotPolicy : ISnapshotPolicy
    {
        private readonly int _version;

        public ExplicitVersionSnapshotPolicy(int version)
        {
            _version = version;
        }

        public bool Check(IEventStream eventStream)
        {
            return false;// return eventStream. == _version;
        }
    }
}
