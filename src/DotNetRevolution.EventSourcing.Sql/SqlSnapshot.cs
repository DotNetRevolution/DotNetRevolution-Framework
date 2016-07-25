namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSnapshot
    {
        public byte[] TypeId { get; }

        public byte[] Data { get; }

        public SqlSnapshot(byte[] typeId, byte[] data)
        {
            TypeId = typeId;
            Data = data;
        }
    }
}
