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

        public SnapshotRevision(Snapshot snapshot)
            : this(EventProviderVersion.Initial, snapshot, false)
        {
            Contract.Requires(snapshot != null);
        }

        public SnapshotRevision(EventProviderVersion version, Snapshot snapshot)
            : this(version, snapshot, false)
        {
            Contract.Requires(version != null);
            Contract.Requires(snapshot != null);
        }

        public SnapshotRevision(EventProviderVersion version, Snapshot snapshot, bool committed)
            : base(version, committed)
        {
            Contract.Requires(version != null);
            Contract.Requires(snapshot != null);

            _snapshot = snapshot;
        }
    }
}
