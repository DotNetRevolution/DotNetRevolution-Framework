namespace DotNetRevolution.EventSourcing
{
    public interface IEventStreamProcessor
    {
        TAggregateRoot Process<TAggregateRoot>(EventStream eventStream) where TAggregateRoot : class;
    }
}