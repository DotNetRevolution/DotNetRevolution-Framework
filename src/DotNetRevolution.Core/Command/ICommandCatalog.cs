using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandCatalogContract))]
    public interface ICommandCatalog
    {
        [Pure]
        IReadOnlyCollection<Type> CommandTypes { get; }

        [Pure]
        ICommandEntry this[Type commandType] { get; }

        void Add(ICommandEntry entry);
    }
}
