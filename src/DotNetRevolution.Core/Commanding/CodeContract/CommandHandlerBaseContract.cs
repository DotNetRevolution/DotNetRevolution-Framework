using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(CommandHandler<>))]
    internal abstract class CommandHandlerBaseContract<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override void Handle(TCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
