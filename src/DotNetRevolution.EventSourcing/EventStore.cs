using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Serialization;
using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using DotNetRevolution.Core.Persistence;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private const string ErrorGettingEventStream = "Error processing request to get event stream.";
        private const string ErrorCommittingTransaction = "Error processing request to commit transaction.";

        private readonly IUsernameProvider _usernameProvider;

        public EventStore(IUsernameProvider usernameProvider,
                          ISerializer serializer)
        {     
            Contract.Requires(serializer != null);
            Contract.Requires(usernameProvider != null);
                     
            _usernameProvider = usernameProvider;
        }

        public IEventStream GetEventStream<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {            
            try
            {
                // create new event provider type
                var eventProviderType = new EventProviderType(typeof(TAggregateRoot));
                                
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

        public async Task<IEventStream> GetEventStreamAsync<TAggregateRoot>(Identity identity) where TAggregateRoot : class, IAggregateRoot
        {
            try
            {
                // create new event provider type
                var eventProviderType = new EventProviderType(typeof(TAggregateRoot));

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
            }
            catch (ConcurrencyException)
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
                var uncommittedRevisions = transaction.EventStream.GetUncommittedRevisions();
                Contract.Assume(uncommittedRevisions != null);

                await CommitTransactionAsync(_usernameProvider.GetUsername(), transaction, uncommittedRevisions);

                uncommittedRevisions.ForEach(x => x.Commit());
            }
            catch (ConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new EventStoreException(ErrorCommittingTransaction, ex);
            }
        }

        protected abstract EventStream GetEventStream(EventProviderType eventProviderType, Identity identity);

        protected abstract Task<EventStream> GetEventStreamAsync(EventProviderType eventProviderType, Identity identity);

        protected abstract void CommitTransaction(string username, EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions);
        protected abstract Task CommitTransactionAsync(string username, EventProviderTransaction transaction, IReadOnlyCollection<EventStreamRevision> uncommittedRevisions);
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {            
            Contract.Invariant(_usernameProvider != null);
        }        
    }
}
