using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.CodeContract;
using System;
using System.Collections.Generic;
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
        IEventStream GetEventStream<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot;

        [Pure]
        Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot;

        [Pure]
        ICollection<EventProviderTransactionCollection> GetTransactions<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot;

        [Pure]
        Task<ICollection<EventProviderTransactionCollection>> GetTransactionsAsync<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot;

        event EventHandler<TransactionCommittedEventArgs> TransactionCommitted;
    }
}