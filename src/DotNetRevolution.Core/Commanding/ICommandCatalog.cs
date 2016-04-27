using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandCatalogContract))]
    public interface ICommandCatalog
    {
        IReadOnlyCollection<Type> CommandTypes { [Pure] get; }

        ICommandCatalog Add(ICommandEntry entry);

        [Pure]
        ICommandEntry GetEntry(Type commandType);
    }
}
