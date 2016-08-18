using System;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlDomainEvent
    {
        public int EventProviderVersion { get; }

        public int Sequence { get; }

        public byte[] EventTypeId { get; }

        public byte[] Data { get; }

        public DateTime Committed { get; }

        public SqlDomainEvent(int eventProviderVersion,
                              int sequence,
                              DateTime committed,
                              byte[] eventTypeId,
                              byte[] data)
        {
            EventProviderVersion = eventProviderVersion;
            Sequence = sequence;
            Committed = committed;
            EventTypeId = eventTypeId;
            Data = data;
        }
    }
}
