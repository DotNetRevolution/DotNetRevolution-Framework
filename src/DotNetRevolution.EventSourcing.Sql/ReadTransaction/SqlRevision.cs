using System;

namespace DotNetRevolution.EventSourcing.Sql.ReadTransaction
{
    internal class SqlRevision
    {
        public Guid EventProviderId { get; }

        public Guid AggregateRootId { get; }

        public Guid EventProviderRevisionId { get; }

        public Guid TransactionId { get; }

        public int EventProviderVersion { get; }

        public byte[] Metadata { get; }
        
        public byte[] CommandTypeId { get; }

        public byte[] CommandData { get; }

        public string Descriptor { get; }

        public SqlRevision(Guid eventProviderId, Guid aggregateRootId, string descriptor, Guid eventProviderRevisionId, int eventProviderVersion, Guid transactionId,
             byte[] metadata, byte[] commandTypeId, byte[] commandData)
        {
            EventProviderId = eventProviderId;
            Descriptor = descriptor;
            AggregateRootId = aggregateRootId;
            EventProviderRevisionId = eventProviderRevisionId;
            TransactionId = transactionId;
            EventProviderVersion = eventProviderVersion;
            Metadata = metadata;
            CommandTypeId = commandTypeId;
            CommandData = commandData;
        }
    }
}
