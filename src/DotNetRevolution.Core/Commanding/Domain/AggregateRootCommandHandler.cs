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

        public override ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context)
        {
            var command = context.Command;
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // create identity from command
            return Handle(context, new AggregateRootIdentity(command.AggregateRootId));
        }

        protected ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context, AggregateRootIdentity identity)
        {
            Contract.Requires(context != null);
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);
            
            // use identity to get aggregate root
            TAggregateRoot aggregateRoot = GetAggregateRoot(identity);

            // execute command
            aggregateRoot.Execute(context.Command);

            // commit changes
            return _repository.Commit(context, aggregateRoot);
        }

        public override Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context)
        {
            var command = context.Command;
            Contract.Assume(command.AggregateRootId != Guid.Empty);

            // create identity from command
            return HandleAsync(context, new AggregateRootIdentity(command.AggregateRootId));
        }

        protected async Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context, AggregateRootIdentity identity)
        {
            Contract.Requires(context != null);
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            
            // use identity to get aggregate root
            TAggregateRoot aggregateRoot = await GetAggregateRootAsync(identity);

            // execute command
            aggregateRoot.Execute(context.Command);

            // commit changes
            return await _repository.CommitAsync(context, aggregateRoot);
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
