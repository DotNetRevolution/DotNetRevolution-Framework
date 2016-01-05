using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Command.CodeContract;

namespace DotNetRevolution.Core.Command
{
    [ContractClass(typeof(CommandAwareFactoryContract<>))]
    public interface ICommandAwareFactory<out T>
        where T : class
    {
        [Pure]
        Type Type { get; }

        T Create(object command);
    }
}
