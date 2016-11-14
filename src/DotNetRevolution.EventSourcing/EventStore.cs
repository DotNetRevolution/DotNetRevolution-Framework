using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Serialization;
using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using DotNetRevolution.Core.Persistence;
using System.Threading.Tasks;
using System.Linq;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private const string ErrorGettingEventStream = "Error processing request to get event stream.";
        private const string ErrorCommittingTransaction = "Error processing request to commit transaction.";

        private readonly IUsernameProvider _usernameProvider;

        public event EventHandler<TransactionCommittedEventArgs> TransactionCommitted = (s, e) => { };

        public EventStore(IUsernameProvider usernameProvider,
                          ISerializer serializer)
        {     
            Contract.Requires(serializer != null);
            Contract.Requires(usernameProvider != null);
                     
            _usernameProvider = usernameProvider;
        }

        protected abstract EventStream GetEventStream(AggregateRootType eventProviderType, AggregateRootIdentity aggregateRootIdentity);

        protected abstract Task<EventStream> GetEventStreamAsync(AggregateRootType eventProviderType, AggregateRootIdentity aggregateRootIdentity);

        protected abstract void CommitTransaction(string username, EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions);

        protected abstract Task CommitTransactionAsync(string username, EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions);
        
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

        public void Commit(EventProviderTransaction transaction)
        {
            try
            {
                var uncommittedRevisions = transaction.EventStream.GetUncommittedRevisions();
                Contract.Assume(uncommittedRevisions != null);

                CommitTransaction(_usernameProvider.GetUsername(), transaction, uncommittedRevisions);

                uncommittedRevisions.ForEach(x => x.Commit());

                RaiseTransactionCommitted(transaction, uncommittedRevisions);
            }
            catch (AggregateRootConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorCommittingTransaction, ex);
            }

            Contract.Assume(transaction.EventStream.GetUncommittedRevisions()?.Count == 0);
        }

        public async Task CommitAsync(EventProviderTransaction transaction)
        {
            try
            {
                var uncommittedRevisions = transaction.EventStream.GetUncommittedRevisions();
                Contract.Assume(uncommittedRevisions != null);

                await CommitTransactionAsync(_usernameProvider.GetUsername(), transaction, uncommittedRevisions);

                uncommittedRevisions.ForEach(x => x.Commit());

                RaiseTransactionCommitted(transaction, uncommittedRevisions);
            }
            catch (AggregateRootConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorCommittingTransaction, ex);
            }

            Contract.Assume(transaction.EventStream.GetUncommittedRevisions()?.Count == 0);
        }

        private void RaiseTransactionCommitted(EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions)
        {
            Contract.Requires(transaction != null);
            Contract.Requires(uncommittedRevisions != null);

            var domainEventRevisions = uncommittedRevisions.Where(x => x is DomainEventRevision).Cast<DomainEventRevision>().ToList();
            var domainEvents = domainEventRevisions.Aggregate(new List<IDomainEvent>(), (seed, revision) =>
            {
                seed.AddRange(revision.DomainEvents);
                return seed;
            });
            Contract.Assume(domainEvents != null);

            TransactionCommitted(this, new TransactionCommittedEventArgs(transaction, domainEvents));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_usernameProvider != null);
            Contract.Invariant(TransactionCommitted != null);
        }        
    }
}
