namespace DotNetRevolution.EventSourcing.Sql.ReadDomainEvent
{
    internal class SqlSnapshot
    {
        public byte[] TypeId { get; }

        public byte[] Data { get; }

        public EventProviderVersion Version { get; }

        public EventStreamRevisionIdentity Identity { get; }
        
        public SqlSnapshot(EventStreamRevisionIdentity identity, EventProviderVersion version, byte[] typeId, byte[] data)
        {
            Identity = identity;
            Version = version;
            TypeId = typeId;
            Data = data;
        }
    }
}
