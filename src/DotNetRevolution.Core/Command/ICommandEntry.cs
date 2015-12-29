using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandEntryContract))]
    public interface ICommandEntry
    {
        Type CommandType { [Pure] get; }
        Type CommandHandlerType { [Pure] get; }
        ICommandHandler CommandHandler { [Pure] get; set; }
    }
}