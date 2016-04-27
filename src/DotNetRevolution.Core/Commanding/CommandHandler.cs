using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    [ContractClass(typeof(CommandHandlerBaseContract<>))]
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public virtual bool Reusable
        {
            get { return true; }
        }

        public abstract void Handle(TCommand command);

        public void Handle(ICommand command)
        {
            Handle((TCommand) command);
        }
    }
}
