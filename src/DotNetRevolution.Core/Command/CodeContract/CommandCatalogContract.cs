using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command.CodeContract
{
    [ContractClassFor(typeof(ICommandCatalog))]
    public abstract class CommandCatalogContract : ICommandCatalog
    {
        public IReadOnlyCollection<Type> CommandTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<Type>>() != null);

                throw new NotImplementedException();
            }
        }

        public ICommandEntry this[Type commandType]
        {
            get
            {
                Contract.Requires(commandType != null);

                throw new NotImplementedException();
            }
        }

        public void Add(ICommandEntry entry)
        {
            Contract.Requires(entry != null);
            Contract.Ensures(this[entry.CommandType] != null);         
        }
    }
}
