using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(IRepository<>))]
    internal abstract class RepositoryContract<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public ICommandHandlingResult Commit(ICommand command, TAggregateRoot aggregateRoot)
        {
            Contract.Requires(command != null);
            Contract.Requires(aggregateRoot != null);
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            throw new NotImplementedException();
        }

        public TAggregateRoot GetByIdentity(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }
        public Task<ICommandHandlingResult> CommitAsync(ICommand command, TAggregateRoot aggregateRoot)
        {
            Contract.Requires(command != null);
            Contract.Requires(aggregateRoot != null);
            Contract.Ensures(Contract.Result<Task<ICommandHandlingResult>>() != null);

            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> GetByIdentityAsync(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<Task<TAggregateRoot>>() != null);

            throw new NotImplementedException();
        }
    }
}
