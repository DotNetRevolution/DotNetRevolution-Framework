using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStoreContract))]
    public interface IEventStore
    {
        void Commit(EventProviderTransaction transaction);

        [Pure]
        EventProvider<TAggregateRoot> GetEventProvider<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot;
    }
}