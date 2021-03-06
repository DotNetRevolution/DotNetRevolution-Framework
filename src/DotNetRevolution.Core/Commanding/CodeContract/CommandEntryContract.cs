﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandEntry))]
    internal abstract class CommandEntryContract : ICommandEntry
    {
        public Type CommandType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public Type CommandHandlerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
