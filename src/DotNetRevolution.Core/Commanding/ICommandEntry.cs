using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Commanding.CodeContract;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandEntryContract))]
    public interface ICommandEntry
    {
        Type CommandType { [Pure] get; }
        Type CommandHandlerType { [Pure] get; }
    }
}
