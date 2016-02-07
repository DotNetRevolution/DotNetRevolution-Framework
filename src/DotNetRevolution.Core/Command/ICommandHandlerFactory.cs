using DotNetRevolution.Core.Command.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandHandlerFactoryContract))]
    public interface ICommandHandlerFactory
    {
        ICommandCatalog Catalog { [Pure] get; }

        ICommandHandler GetHandler(object command);
    }
}
