using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Messaging;
using DotNetRevolution.EventSourcing.Commanding.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Commanding
{
    [ContractClass(typeof(EventStoreCommandHandlerContract<,>))]
    public abstract class EventStoreCommandHandler<TAggregateRoot, TCommand> : CommandHandler<TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : ICommand
    {
        private readonly IEventStore _eventStore;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IMessageDispatcher _messageDispatcher;

        public EventStoreCommandHandler(IEventStore eventStore,
                                        IDomainEventDispatcher domainEventDispatcher,
                                        IMessageDispatcher messageDispatcher)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(domainEventDispatcher != null);
            Contract.Requires(messageDispatcher != null);

            _eventStore = eventStore;
            _domainEventDispatcher = domainEventDispatcher;
            _messageDispatcher = messageDispatcher;
        }

        protected abstract Identity GetEventProviderIdentity(TCommand command);
        protected abstract IDomainEventCollection Handle(TAggregateRoot aggregateRoot, TCommand command);

        public override void Handle(TCommand command)
        {
            Contract.Assume(command != null);

            var identity = GetEventProviderIdentity(command);

            var eventProvider = _eventStore.GetEventProvider<TAggregateRoot>(identity);

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            var domainEvents = Handle(aggregateRoot, command);
            Contract.Assume(domainEvents.AggregateRoot != null);
            Contract.Assume(string.IsNullOrWhiteSpace(domainEvents.AggregateRoot.ToString()) == false);
            Contract.Assume(Contract.ForAll(domainEvents, o => o != null));

            var transaction = new Transaction("d", command, eventProvider.CreateNewVersion(domainEvents));

            _eventStore.Commit(transaction);
            
            _domainEventDispatcher.PublishAll(domainEvents);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_eventStore != null);
            Contract.Invariant(_domainEventDispatcher != null);
            Contract.Invariant(_messageDispatcher != null);
        }
    }
}
