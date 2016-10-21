using DotNetRevolution.Core.Commanding.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandContract))]
    public interface ICommand
    {
        Guid CommandId { [Pure] get; }
    }
}
