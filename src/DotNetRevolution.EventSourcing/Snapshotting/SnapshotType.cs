using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Snapshotting
{
    public class SnapshotType : ValueObject<SnapshotType>
    {
        public Type Type { get; }

        public SnapshotType(Type snapshotType)
        {            
            Contract.Requires(snapshotType != null);

            Type = snapshotType;
        }
    }
}
