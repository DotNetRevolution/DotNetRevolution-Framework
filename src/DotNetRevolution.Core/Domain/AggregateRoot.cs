namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Identity Identity { get; }
    }
}
