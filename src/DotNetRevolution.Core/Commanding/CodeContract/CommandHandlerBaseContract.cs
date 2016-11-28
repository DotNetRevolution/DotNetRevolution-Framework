using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(CommandHandler<>))]
    internal abstract class CommandHandlerBaseContract<TCommand> : CommandHandler<TCommand>
        where TCommand : ICommand
    {
        public override ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context)
        {
            Contract.Ensures(Contract.Result<ICommandHandlingResult>() != null);

            throw new NotImplementedException();
        }
    }
}
