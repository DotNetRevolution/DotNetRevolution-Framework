namespace DotNetRevolution.EventSourcing
{
    public interface IEventStreamRevision
    {
        bool Committed { get; }

        EventProviderVersion Version { get; }
    }
}