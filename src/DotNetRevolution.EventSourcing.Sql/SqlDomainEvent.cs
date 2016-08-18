using System;

namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlDomainEvent
    {
        public int EventProviderVersion { get; }

        public int Sequence { get; }

        public byte[] EventTypeId { get; }

        public byte[] Data { get; }
        
        public SqlDomainEvent(int eventProviderVersion,
                              int sequence,
                              byte[] eventTypeId,
                              byte[] data)
        {
            EventProviderVersion = eventProviderVersion;
            Sequence = sequence;
            EventTypeId = eventTypeId;
            Data = data;
        }
    }
}
