namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSnapshot
    {
        public byte[] TypeId { get; }

        public byte[] Data { get; }

        public EventProviderVersion Version { get; }
        
        public SqlSnapshot(EventProviderVersion version, byte[] typeId, byte[] data)
        {
            Version = version;
            TypeId = typeId;
            Data = data;
        }
    }
}
