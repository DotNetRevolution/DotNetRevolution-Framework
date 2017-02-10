using System;

namespace DotNetRevolution.EventSourcing.Sql.ReadDomainEvent
{
    internal class SqlDomainEvent
    {
        public Guid EventProviderRevisionId { get; }

        public int EventProviderVersion { get; }

        public int Sequence { get; }

        public byte[] EventTypeId { get; }

        public byte[] Data { get; }
        
        public SqlDomainEvent(Guid eventProviderRevisionId, 
                              int eventProviderVersion,
                              int sequence,
                              byte[] eventTypeId,
                              byte[] data)
        {
            EventProviderRevisionId = eventProviderRevisionId;
            EventProviderVersion = eventProviderVersion;
            Sequence = sequence;
            EventTypeId = eventTypeId;
            Data = data;
        }
    }
}
