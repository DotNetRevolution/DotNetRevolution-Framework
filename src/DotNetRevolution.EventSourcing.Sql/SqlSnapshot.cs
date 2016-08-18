using System;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSnapshot
    {
        public byte[] TypeId { get; }

        public byte[] Data { get; }

        public EventProviderVersion Version { get; }

        public DateTime Committed { get; }

        public SqlSnapshot(EventProviderVersion version, DateTime committed, byte[] typeId, byte[] data)
        {
            Version = version;
            Committed = committed;
            TypeId = typeId;
            Data = data;
        }
    }
}
