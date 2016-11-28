using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding.CodeContract
{
    [ContractClassFor(typeof(ICommandHandlerContextFactory))]
    internal abstract class CommandHandlerContextFactoryContract : ICommandHandlerContextFactory
    {
        public ICommandHandlerContext GetContext(ICommand command)
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandlerContext>() != null);

            throw new NotImplementedException();
        }

        public ICommandHandlerContext<TCommand> GetContext<TCommand>(TCommand command) where TCommand : ICommand
        {
            Contract.Requires(command != null);
            Contract.Ensures(Contract.Result<ICommandHandlerContext<TCommand>>() != null);

            throw new NotImplementedException();
        }
    }
}
