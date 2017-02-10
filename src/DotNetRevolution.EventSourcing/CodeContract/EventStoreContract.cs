using System;
using System.Collections.Generic;
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

            // static checker fails when using ForAll
            //Contract.Ensures(Contract.ForAll(transaction.Revisions, o => o.Committed));
            //Contract.EnsuresOnThrow<Exception>(Contract.ForAll(transaction.Revisions, o => o.Committed == false));
        }

        public Task CommitAsync(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public IEventStream GetEventStream<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<IEventStream> () != null);

            throw new NotImplementedException();
        }

        public Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task<IEventStream>>() != null);

            throw new NotImplementedException();
        }

        public ICollection<EventProviderTransactionCollection> GetTransactions<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(eventProvidersToSkip >= 0);
            Contract.Requires(eventProvidersToTake > 0);
            Contract.Ensures(Contract.Result<ICollection<EventProviderTransactionCollection>>() != null);

            throw new NotImplementedException();
        }

        public Task<ICollection<EventProviderTransactionCollection>> GetTransactionsAsync<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot
        {
            Contract.Requires(eventProvidersToSkip >= 0);
            Contract.Requires(eventProvidersToTake > 0);
            Contract.Ensures(Contract.Result<Task<ICollection<EventProviderTransactionCollection>>>() != null);

            throw new NotImplementedException();
        }        
    }
}
