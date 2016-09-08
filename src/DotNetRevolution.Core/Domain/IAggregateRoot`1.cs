namespace DotNetRevolution.Core.Domain
{
    public interface IAggregateRoot<TAggregateRootState> : IAggregateRoot
        where TAggregateRootState : IAggregateRootState
    {
        new TAggregateRootState State { get; }
    }
}
