namespace DotNetRevolution.Core.Domain
{
    public interface IAggregateRootState : IStateTracker
    {
        IStateTracker InternalStateTracker { get; set; }
    }
}
