using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
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
            var identity = new Identity(command.AggregateRootId);

            // use identity to get aggregate root
            TAggregateRoot aggregateRoot = GetAggregateRoot(identity);

            // execute command
            aggregateRoot.Execute(command);

            // commit changes
            _repository.Commit(command, aggregateRoot);
        }

        protected virtual TAggregateRoot GetAggregateRoot(Identity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            // get aggregate root
            return _repository.GetByIdentity(identity);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_repository != null);
        }
    }
}
