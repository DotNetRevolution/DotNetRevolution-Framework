﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandAwareFactory<>))]
    internal abstract class CommandAwareFactoryContract<T> : ICommandAwareFactory<T>
        where T : class
    {
        public Type Type
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public T Get(object command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<T>() != null);

            throw new NotImplementedException();
        }
    }
}
