using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.Domain
{
    public class AggregateRootCommandHandler<TAggregateRoot, TCommand> : CommandHandler<TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : IAggregateRootCommand<TAggregateRoot>
    {
        private readonly IRepository<TAggregateRoot> _repository;

        public AggregateRootCommandHandler(IRepository<TAggregateRoot> repository)
        {
            Contract.Requires(repository != null);

            _repository = repository;
        }

        public override void Handle(TCommand command)
        {
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // create identity from command
            Handle(command, new AggregateRootIdentity(command.AggregateRootId));
        }

        protected void Handle(TCommand command, AggregateRootIdentity identity)
        {
            Contract.Requires(command != null);
            Contract.Requires(identity != null);

            // use identity to get aggregate root
            TAggregateRoot aggregateRoot = GetAggregateRoot(identity);

            // execute command
            aggregateRoot.Execute(command);

            // commit changes
            _repository.Commit(command, aggregateRoot);
        }

        public override Task HandleAsync(TCommand command)
        {
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // create identity from command
            return HandleAsync(command, new AggregateRootIdentity(command.AggregateRootId));
        }

        protected async Task HandleAsync(TCommand command, AggregateRootIdentity identity)
        {
            Contract.Requires(command != null);
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            // use identity to get aggregate root
            TAggregateRoot aggregateRoot = await GetAggregateRootAsync(identity);

            // execute command
            aggregateRoot.Execute(command);

            // commit changes
            await _repository.CommitAsync(command, aggregateRoot);
        }

        protected virtual TAggregateRoot GetAggregateRoot(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            // get aggregate root
            return _repository.GetByIdentity(identity);
        }

        protected virtual Task<TAggregateRoot> GetAggregateRootAsync(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task<TAggregateRoot>>() != null);

            // get aggregate root
            return _repository.GetByIdentityAsync(identity);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_repository != null);
        }
    }
}
