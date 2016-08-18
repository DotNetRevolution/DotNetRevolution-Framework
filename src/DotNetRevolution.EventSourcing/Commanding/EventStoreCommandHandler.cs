using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.EventSourcing.Commanding.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Commanding
{
    [ContractClass(typeof(EventStoreCommandHandlerContract<,>))]
    public abstract class EventStoreCommandHandler<TAggregateRoot, TCommand> : CommandHandler<TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : ICommand
    {
        private readonly IEventProviderRepository<TAggregateRoot> _repository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IAggregateRootProcessor _processor;

        public EventStoreCommandHandler(IEventProviderRepository<TAggregateRoot> repository,
                                        IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(repository != null);
            Contract.Requires(domainEventDispatcher != null);

            _repository = repository;
            _domainEventDispatcher = domainEventDispatcher;
        }

        protected abstract Identity GetEventProviderIdentity(TCommand command);
        protected abstract IDomainEventCollection Handle(TAggregateRoot aggregateRoot, TCommand command);

        public override void Handle(TCommand command)
        {
            Contract.Assume(command != null);

            // get identity from command
            var identity = GetEventProviderIdentity(command);

            TAggregateRoot aggregateRoot;

            // get event stream and aggregate root
            var eventStream = _repository.GetByIdentity(GetEventProviderIdentity(command), out aggregateRoot);
                
            // handle command returning domain events
            var domainEvents = Handle(aggregateRoot, command);
            Contract.Assume(domainEvents.AggregateRoot != null);
            Contract.Assume(string.IsNullOrWhiteSpace(domainEvents.AggregateRoot.ToString()) == false);
            Contract.Assume(Contract.ForAll(domainEvents, o => o != null));

            // append domain events
            eventStream.Append(domainEvents);

            // make sure uncommitted revisions meets criteria for committing
            var uncommittedRevisions = eventStream.GetUncommittedRevisions();
            Contract.Assume(uncommittedRevisions != null);
            Contract.Assume(uncommittedRevisions.Count > 0);

            // commit stream
            _repository.Commit(command, eventStream, aggregateRoot);
            
            // publish all domain events on dispatcher
            _domainEventDispatcher.PublishAll(domainEvents);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_repository != null);
            Contract.Invariant(_domainEventDispatcher != null);
        }
    }
}
