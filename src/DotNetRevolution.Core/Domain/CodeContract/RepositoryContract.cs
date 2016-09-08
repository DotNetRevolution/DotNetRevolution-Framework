using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IRepository<>))]
    internal abstract class RepositoryContract<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public void Commit(TAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);

            throw new NotImplementedException();
        }

        public TAggregateRoot GetByIdentity(Identity identity)
        {
            Contract.Requires(identity != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }
    }
}
