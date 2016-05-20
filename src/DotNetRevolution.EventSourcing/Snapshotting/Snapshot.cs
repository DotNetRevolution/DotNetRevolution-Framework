using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class Snapshot
    {
        private readonly object _data;

        public SnapshotType SnapshotType { [Pure] get; }

        public object Data
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                return _data;
            }
        }

        public Snapshot(object data)
        {
            Contract.Requires(data != null);

            _data = data;
            SnapshotType = new SnapshotType(data.GetType());
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_data != null);
        }
    }
}
