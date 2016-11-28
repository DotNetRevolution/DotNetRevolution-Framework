using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotRevision : EventStreamRevision
    {
        private readonly Snapshot _snapshot;

        [Pure]
        public Snapshot Snapshot
        {
            get
            {
                Contract.Ensures(Contract.Result<Snapshot>() != null);
                return _snapshot;
            }
        }
                        
        public SnapshotRevision(EventStreamRevisionIdentity identity, EventProviderVersion version, Snapshot snapshot)
            : base(identity, version)
        {
            Contract.Requires(identity != null);
            Contract.Requires(version != null);
            Contract.Requires(snapshot != null);

            _snapshot = snapshot;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_snapshot != null);
        }
    }
}
