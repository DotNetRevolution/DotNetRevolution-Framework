using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootStateBuilder<>))]
    internal abstract class AggregateRootStateBuilderContract<TAggregateRootState> : IAggregateRootStateBuilder<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        public TAggregateRootState Build(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);
            Contract.Ensures(Contract.Result<TAggregateRootState>() != null);

            throw new NotImplementedException();
        }

        public TAggregateRootState Build(object snapshot)
        {
            Contract.Requires(snapshot != null);
            Contract.Ensures(Contract.Result<TAggregateRootState>() != null);

            throw new NotImplementedException();
        }

        public TAggregateRootState Build(object snapshot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(snapshot != null);
            Contract.Requires(domainEvents != null);
            Contract.Ensures(Contract.Result<TAggregateRootState>() != null);

            throw new NotImplementedException();
        }
    }
}
