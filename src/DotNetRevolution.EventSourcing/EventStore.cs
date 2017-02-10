using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Serialization;
using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private const string ErrorGettingEventStream = "Error processing request to get event stream.";
        private const string ErrorCommittingTransaction = "Error processing request to commit transaction.";
        private const string ErrorGettingTransactionCollections = "Error processing request to get transaction collections.";

        public event EventHandler<TransactionCommittedEventArgs> TransactionCommitted = (s, e) => { };

        protected EventStore(ISerializer serializer)
        {     
            Contract.Requires(serializer != null);
        }

        protected abstract EventStream GetEventStream(AggregateRootType eventProviderType, AggregateRootIdentity aggregateRootIdentity);

        protected abstract Task<EventStream> GetEventStreamAsync(AggregateRootType eventProviderType, AggregateRootIdentity aggregateRootIdentity);

        protected abstract ICollection<EventProviderTransactionCollection> GetTransactions(AggregateRootType eventProviderType, int eventProvidersToSkip, int eventProvidersToTake);

        protected abstract Task<ICollection<EventProviderTransactionCollection>> GetTransactionsAsync(AggregateRootType eventProviderType, int eventProvidersToSkip, int eventProvidersToTake);

        protected abstract void CommitTransaction(EventProviderTransaction transaction);

        protected abstract Task CommitTransactionAsync(EventProviderTransaction transaction);
        
        public IEventStream GetEventStream<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot
        {            
            try
            {
                // create new event provider type
                var eventProviderType = new AggregateRootType(typeof(TAggregateRoot));
                                
                // get event stream
                var eventStream = GetEventStream(eventProviderType, identity);
                Contract.Assume(eventStream != null);

                // return event stream
                return eventStream;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorGettingEventStream, ex);
            }
        }

        public async Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(AggregateRootIdentity identity) where TAggregateRoot : class, IAggregateRoot
        {
            try
            {
                // create new event provider type
                var eventProviderType = new AggregateRootType(typeof(TAggregateRoot));

                // get event stream
                var eventStream = await GetEventStreamAsync(eventProviderType, identity);
                Contract.Assume(eventStream != null);

                // return event stream
                return eventStream;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorGettingEventStream, ex);
            }
        }

        public ICollection<EventProviderTransactionCollection> GetTransactions<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot
        {
            try
            {
                // create new event provider type
                var eventProviderType = new AggregateRootType(typeof(TAggregateRoot));

                // get transactions
                var transactions = GetTransactions(eventProviderType, eventProvidersToSkip, eventProvidersToTake);
                Contract.Assume(transactions != null);

                // return transactions
                return transactions;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorGettingTransactionCollections, ex);
            }
        }

        public async Task<ICollection<EventProviderTransactionCollection>> GetTransactionsAsync<TAggregateRoot>(int eventProvidersToSkip, int eventProvidersToTake) where TAggregateRoot : class, IAggregateRoot
        {
            try
            {
                // create new event provider type
                var eventProviderType = new AggregateRootType(typeof(TAggregateRoot));

                // get transactions
                var transactions = await GetTransactionsAsync(eventProviderType, eventProvidersToSkip, eventProvidersToTake);
                Contract.Assume(transactions != null);

                // return transactions
                return transactions;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorGettingTransactionCollections, ex);
            }
        }

        public void Commit(EventProviderTransaction transaction)
        {
            try
            {
                CommitTransaction(transaction);

                // raise transaction committed event for any listeners
                TransactionCommitted(this, new TransactionCommittedEventArgs(transaction));
            }
            catch (AggregateRootConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorCommittingTransaction, ex);
            }
        }

        public async Task CommitAsync(EventProviderTransaction transaction)
        {
            try
            {
                await CommitTransactionAsync(transaction);

                // raise transaction committed event for any listeners
                TransactionCommitted(this, new TransactionCommittedEventArgs(transaction));
            }
            catch (AggregateRootConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorCommittingTransaction, ex);
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(TransactionCommitted != null);
        }
    }
}
