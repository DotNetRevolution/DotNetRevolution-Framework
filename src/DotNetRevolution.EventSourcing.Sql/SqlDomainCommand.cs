namespace DotNetRevolution.EventSourcing.Sql
{
    internal class SqlDomainCommand
    {
        public string FullName { get; }

        public byte[] Data { get; }

        public SqlDomainCommand(string fullName, byte[] data)
        {
            FullName = fullName;
            Data = data;
        }
    }
}
