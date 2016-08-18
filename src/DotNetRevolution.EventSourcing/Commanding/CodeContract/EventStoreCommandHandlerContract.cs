using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.EventSourcing.Commanding.CodeContract
{
    [ContractClassFor(typeof(EventStoreCommandHandler<,>))]
    internal abstract class EventStoreCommandHandlerContract<TAggregateRoot, TCommand> : EventStoreCommandHandler<TAggregateRoot, TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : ICommand
    {
        public EventStoreCommandHandlerContract(IEventProviderRepository<TAggregateRoot> repository,
                                                IDomainEventDispatcher domainEventDispatcher)
            : base(repository, domainEventDispatcher)
        {
            Contract.Requires(repository != null);
            Contract.Requires(domainEventDispatcher != null);
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
