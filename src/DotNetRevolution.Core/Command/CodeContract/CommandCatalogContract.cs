﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandCatalog))]
    internal abstract class CommandCatalogContract : ICommandCatalog
    {
        public IReadOnlyCollection<Type> CommandTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Type>>() != null);

                throw new NotImplementedException();
            }
        }
        
        public void Add(ICommandEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(GetEntry(entry.CommandType) != null);
        }

        [Pure]
        public ICommandEntry GetEntry(Type commandType)
        {
            Contract.Requires(commandType != null);

            throw new NotImplementedException();
        }
    }
}
