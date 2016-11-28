using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Commanding.Domain.CodeContract
{
    [ContractClassFor(typeof(IRepository<>))]
    internal abstract class RepositoryContract<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public ICommandHandlingResult Commit(ICommandHandlerContext context, TAggregateRoot aggregateRoot)
        {
            Contract.Requires(context != null);
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
        public Task<ICommandHandlingResult> CommitAsync(ICommandHandlerContext context, TAggregateRoot aggregateRoot)
        {
            Contract.Requires(context != null);
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
