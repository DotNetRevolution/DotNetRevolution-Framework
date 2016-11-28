using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing
{
    public class EventStoreRepository<TAggregateRoot, TAggregateRootState> : Core.Commanding.Domain.IRepository<TAggregateRoot>
        where TAggregateRootState : class, IAggregateRootState
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventStreamProcessor<TAggregateRoot, TAggregateRootState> _eventStreamProcessor;        
        private readonly IGuidGenerator _guidGenerator;

        public EventStoreRepository(IEventStore eventStore,
                                    IEventStreamProcessor<TAggregateRoot, TAggregateRootState> eventStreamProcessor,
                                    IGuidGenerator guidGenerator)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(eventStreamProcessor != null);
            Contract.Requires(guidGenerator!= null);

            _eventStore = eventStore;
            _eventStreamProcessor = eventStreamProcessor;
            _guidGenerator = guidGenerator;
        }

        public async Task<TAggregateRoot> GetByIdentityAsync(AggregateRootIdentity identity)
        {
            Contract.Assume(identity != null);

            var eventStream = await _eventStore.GetEventStreamAsync<TAggregateRoot>(identity);

            // build aggregate root from event stream
            var aggregateRoot = _eventStreamProcessor.Process(eventStream);

            // set external state tracker
            aggregateRoot.State.ExternalStateTracker = new EventStreamStateTracker(eventStream, _guidGenerator);

            return aggregateRoot;
        }

        public async Task<ICommandHandlingResult> CommitAsync(ICommandHandlerContext context, TAggregateRoot aggregateRoot)
        {
            Contract.Assume(context != null);
            Contract.Assume(aggregateRoot?.State != null);

            var command = context.Command;

            // get state tracker from aggregate root state
            var stateTracker = aggregateRoot.State.ExternalStateTracker as IEventStreamStateTracker;
            Contract.Assume(stateTracker?.Revisions != null);

            // make new transaction identity
            TransactionIdentity transactionIdentity = CreateNewTransactionIdentity();

            // create new transaction
            var transaction = new EventProviderTransaction(transactionIdentity, stateTracker.EventStream.EventProvider, command, aggregateRoot, stateTracker.Revisions, context.Metadata.AsReadOnlyCollection());

            // store transaction
            await _eventStore.CommitAsync(transaction);

            // commit state tracker
            stateTracker.Commit();

            // return result
            return CreateCommandHandlingResult(command, aggregateRoot, transactionIdentity);
        }        

        public TAggregateRoot GetByIdentity(AggregateRootIdentity identity)
        {
            Contract.Assume(identity != null);
                        
            var eventStream = _eventStore.GetEventStream<TAggregateRoot>(identity);

            // build aggregate root from event stream
            var aggregateRoot = _eventStreamProcessor.Process(eventStream);
            
            // set external state tracker
            aggregateRoot.State.ExternalStateTracker = new EventStreamStateTracker(eventStream, _guidGenerator);

            return aggregateRoot;
        }

        public ICommandHandlingResult Commit(ICommandHandlerContext context, TAggregateRoot aggregateRoot)
        {
            Contract.Assume(context?.Command != null);
            Contract.Assume(context.Metadata != null);
            Contract.Assume(aggregateRoot?.State != null);

            var command = context.Command;

            // get state tracker from aggregate root state
            var stateTracker = aggregateRoot.State.ExternalStateTracker as IEventStreamStateTracker;
            Contract.Assume(stateTracker?.Revisions != null);

            // make new transaction identity
            TransactionIdentity transactionIdentity = CreateNewTransactionIdentity();

            // create new transaction
            var transaction = new EventProviderTransaction(transactionIdentity, stateTracker.EventStream.EventProvider, command, aggregateRoot, stateTracker.Revisions, context.Metadata);

            // store transaction
            _eventStore.Commit(transaction);

            // commit state tracker
            stateTracker.Commit();

            // return result
            return CreateCommandHandlingResult(command, aggregateRoot, transactionIdentity);
        }

        private TransactionIdentity CreateNewTransactionIdentity()
        {
            Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

            var transactionGuid = _guidGenerator.Create();
            Contract.Assume(transactionGuid != Guid.Empty);

            var transactionIdentity = new TransactionIdentity(transactionGuid);
            
            return transactionIdentity;
        }
        
        private static ICommandHandlingResult CreateCommandHandlingResult(ICommand command, TAggregateRoot aggregateRoot, TransactionIdentity transactionIdentity)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(command != null);
            Contract.Requires(transactionIdentity != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            var aggregateRootIdentity = aggregateRoot.Identity;
            Contract.Assume(aggregateRootIdentity != null);

            var commandId = command.CommandId;
            Contract.Assume(commandId != Guid.Empty);

            return new EventStoreCommandHandlingResult(commandId, aggregateRootIdentity, transactionIdentity);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
            Contract.Invariant(_eventStreamProcessor != null);
            Contract.Invariant(_guidGenerator != null);
        }
    }
}
