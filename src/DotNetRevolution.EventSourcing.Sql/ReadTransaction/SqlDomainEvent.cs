using System;

namespace DotNetRevolution.EventSourcing.Sql.ReadTransaction
{
    internal class SqlDomainEvent
    {
        public Guid EventProviderRevisionId { get; }
        
        public int Sequence { get; }

        public byte[] EventTypeId { get; }

        public byte[] Data { get; }
        
        public SqlDomainEvent(Guid eventProviderRevisionId, 
                              int sequence,
                              byte[] eventTypeId,
                              byte[] data)
        {
            EventProviderRevisionId = eventProviderRevisionId;
            Sequence = sequence;
            EventTypeId = eventTypeId;
            Data = data;
        }
    }
}
