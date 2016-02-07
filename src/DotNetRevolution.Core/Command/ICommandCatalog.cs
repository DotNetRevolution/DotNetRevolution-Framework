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
        
        void Add(ICommandEntry entry);

        [Pure]
        ICommandEntry GetEntry(Type commandType);
    }
}
