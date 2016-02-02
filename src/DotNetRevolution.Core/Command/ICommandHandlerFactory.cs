using DotNetRevolution.Core.Command.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandHandlerFactoryContract))]
    public interface ICommandHandlerFactory : ICommandAwareFactory<ICommandHandler>
    {
        ICommandCatalog Catalog { [Pure] get; }
    }
}
