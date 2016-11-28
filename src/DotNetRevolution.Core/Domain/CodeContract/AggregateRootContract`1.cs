using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRoot<>))]
    internal abstract class AggregateRootContract<TAggregateRootState> : IAggregateRoot<TAggregateRootState>
        where TAggregateRootState : IAggregateRootState
    {
        public abstract AggregateRootIdentity Identity { get; }

        public TAggregateRootState State
        {
            get
            {
                Contract.Ensures(Contract.Result<TAggregateRootState>() != null);
                                
                throw new NotImplementedException();
            }
        }

        IAggregateRootState IAggregateRoot.State { get; }

        public abstract void Execute(ICommand command);
    }
}
