namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlDomainEvent
    {
        public int EventProviderVersion { get; }

        public int Sequence { get; }

        public string EventType { get; }

        public byte[] Data { get; }

        public SqlDomainEvent(int eventProviderVersion,
                              int sequence,
                              string eventType,
                              byte[] data)
        {
            EventProviderVersion = eventProviderVersion;
            Sequence = sequence;
            EventType = eventType;
            Data = data;
        }
    }
}
