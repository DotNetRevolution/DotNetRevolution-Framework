using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class Snapshot
    {
        public object Data { [Pure] get; }

        public Snapshot(object data)
        {
            Contract.Requires(data != null);

            Data = data;
        }
    }
}
