﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Querying.CodeContract
{
    [ContractClassFor(typeof(IQueryEntry))]
    internal abstract class QueryEntryContract : IQueryEntry
    {
        public Type QueryType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }

        public Type QueryHandlerType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                throw new NotImplementedException();
            }
        }
    }
}
