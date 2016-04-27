using DotNetRevolution.Core.Commanding.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerFactoryContract))]
    public interface ICommandHandlerFactory
    {
        ICommandCatalog Catalog { [Pure] get; }

        ICommandHandler GetHandler(Type commandType);
    }
}
