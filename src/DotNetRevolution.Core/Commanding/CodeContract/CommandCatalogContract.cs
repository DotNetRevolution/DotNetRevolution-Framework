﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
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
        
        public ICommandCatalog Add(ICommandEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(Contract.Result<ICommandCatalog>() != null);
            Contract.Ensures(GetEntry(entry.CommandType) == entry);
            Contract.EnsuresOnThrow<ArgumentException>(GetEntry(entry.CommandType) == Contract.OldValue(GetEntry(entry.CommandType)));

            throw new NotImplementedException();
        }

        [Pure]
        public ICommandEntry GetEntry(Type commandType)
        {
            Contract.Requires(commandType != null);

            throw new NotImplementedException();
        }
    }
}
