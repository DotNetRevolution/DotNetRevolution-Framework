using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Metadata;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandlerContext<>))]
    internal abstract class CommandHandlerContextContract<TCommand> : ICommandHandlerContext<TCommand>
        where TCommand : ICommand
    {
        public TCommand Command
        {
            get
            {
                Contract.Ensures(Contract.Result<TCommand>() != null);

                throw new NotImplementedException();
            }
        }

        public abstract MetaCollection Metadata { get; }

        ICommand ICommandHandlerContext.Command { get; }
    }
}
