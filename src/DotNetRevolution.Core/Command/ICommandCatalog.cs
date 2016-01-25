using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandCatalogContract))]
    public interface ICommandCatalog
    {
        IReadOnlyCollection<Type> CommandTypes { [Pure] get; }
        
        ICommandEntry this[Type commandType] { [Pure] get; }

        void Add(ICommandEntry entry);
    }
}
