namespace DotNetRevolution.Core.Domain
{
    public interface IAggregateRootState : IStateTracker
    {
        IStateTracker ExternalStateTracker { get; set; }
    }
}
