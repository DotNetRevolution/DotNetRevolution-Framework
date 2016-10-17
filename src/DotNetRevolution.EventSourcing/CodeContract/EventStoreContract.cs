using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.CodeContract
{
    [ContractClassFor(typeof(IEventStore))]
    internal abstract class EventStoreContract : IEventStore
    {
        public event EventHandler<TransactionCommittedEventArgs> TransactionCommitted;

        public void Commit(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);
            Contract.Ensures(transaction.EventStream.GetUncommittedRevisions()?.Count == 0);
            Contract.EnsuresOnThrow<Exception>(transaction.EventStream.GetUncommittedRevisions()?.Count > 0);
        }

        public Task CommitAsync(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public IEventStream GetEventStream<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<IEventStream> () != null);

            throw new NotImplementedException();
        }

        public Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task<IEventStream>>() != null);

            throw new NotImplementedException();
        }
    }
}
