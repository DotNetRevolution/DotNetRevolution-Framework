namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlSnapshot
    {
        public string FullName { get; }

        public byte[] Data { get; }

        public SqlSnapshot(string fullName, byte[] data)
        {
            FullName = fullName;
            Data = data;
        }
    }
}
