using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing
{
    [ContractClass(typeof(EventStoreContract))]
    public interface IEventStore
    {
        void Commit(EventProviderTransaction transaction);

        Task CommitAsync(EventProviderTransaction transaction);

        [Pure]
        IEventStream GetEventStream<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot;

        [Pure]
        Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot;
    }
}