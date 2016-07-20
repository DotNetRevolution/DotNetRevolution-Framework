using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System;
using DotNetRevolution.Core.Messaging;

namespace DotNetRevolution.EventSourcing.Commanding.CodeContract
{
    [ContractClassFor(typeof(EventStoreCommandHandler<,>))]
    internal abstract class EventStoreCommandHandlerContract<TAggregateRoot, TCommand> : EventStoreCommandHandler<TAggregateRoot, TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : ICommand
    {
        public EventStoreCommandHandlerContract(IEventStore eventStore,
                                                IDomainEventDispatcher domainEventDispatcher,
                                                IMessageDispatcher messageDispatcher)
            : base(eventStore, domainEventDispatcher, messageDispatcher)
        {
            Contract.Requires(eventStore != null);
            Contract.Requires(domainEventDispatcher != null);
            Contract.Requires(messageDispatcher != null);
        }

        protected override Identity GetEventProviderIdentity(TCommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<Identity>() != null);

            throw new NotImplementedException();
        }

        protected override IDomainEventCollection Handle(TAggregateRoot aggregateRoot, TCommand command)
        {
            Contract.Requires(aggregateRoot != null);
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<IDomainEventCollection>() != null);

            throw new NotImplementedException();
        }
    }
}
