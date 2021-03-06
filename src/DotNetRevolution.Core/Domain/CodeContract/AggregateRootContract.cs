﻿using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding;

namespace DotNetRevolution.Core.Domain.CodeContract
{
    [ContractClassFor(typeof(IAggregateRoot))]
    public abstract class AggregateRootContract : IAggregateRoot
    {
        public AggregateRootIdentity Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<AggregateRootIdentity>() != null);

                throw new NotImplementedException();
            }
        }

        public IAggregateRootState State
        {
            get
            {
                Contract.Ensures(Contract.Result<IAggregateRootState>() != null);

                throw new NotImplementedException();
            }
        }

        public void Execute(ICommand command)
        {
            Contract.Requires(command != null);

            throw new NotImplementedException();
        }
    }
}
