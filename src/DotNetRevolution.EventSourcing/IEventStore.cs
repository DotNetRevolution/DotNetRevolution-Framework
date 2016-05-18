using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing
{
    public interface IEventStore
    {
        void Commit(Transaction transaction);
        EventProvider<TAggregateRoot> GetEventProvider<TAggregateRoot>(Identity identity) where TAggregateRoot : class;
    }
}