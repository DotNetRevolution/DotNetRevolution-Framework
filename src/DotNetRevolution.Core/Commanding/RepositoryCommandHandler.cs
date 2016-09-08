using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class AggregateRootCommandHandler<TAggregateRoot, TCommand> : CommandHandler<TCommand>
        where TAggregateRoot : class, IAggregateRoot
        where TCommand : IAggregateRootCommand
    {
        private readonly IRepository<TAggregateRoot> _repository;

        public AggregateRootCommandHandler(IRepository<TAggregateRoot> repository)
        {
            Contract.Requires(repository != null);

            _repository = repository;
        }

        public override void Handle(TCommand command)
        {
            Contract.Assume(command.Identity != Guid.Empty);

            // get aggregate root
            var aggregateRoot = _repository.GetByIdentity(command.Identity);

            // execute command
            aggregateRoot.Execute(command);

            // commit changes
            _repository.Commit(command, aggregateRoot);
        }        

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_repository != null);
        }
    }
}
