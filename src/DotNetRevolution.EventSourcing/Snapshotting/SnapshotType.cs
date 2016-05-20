using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotType : ValueObject<SnapshotType>
    {
        public string FullName { get; }

        public SnapshotType(Type snapshotType)
        {
            Contract.Requires(snapshotType != null);

            FullName = snapshotType.FullName;
        }
    }
}
