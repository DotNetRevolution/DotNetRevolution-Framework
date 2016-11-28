using DotNetRevolution.Core.Commanding.CodeContract;
using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerContextContract))]
    public interface ICommandHandlerContext
    {
        [Pure]
        ICommand Command { get; }

        [Pure]
        MetaCollection Metadata { get; }
    }
}
