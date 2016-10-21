using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRootBuilder<,>))]
    internal abstract class AggregateRootBuilderContract<TAggregateRoot, TAggregateRootState> : IAggregateRootBuilder<TAggregateRoot, TAggregateRootState>
        where TAggregateRoot : class, IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : class, IAggregateRootState
    {
        public TAggregateRoot Build(AggregateRootIdentity identity, TAggregateRootState state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);
            Contract.Ensures(Contract.Result<TAggregateRoot>() != null);

            throw new NotImplementedException();
        }
    }
}
