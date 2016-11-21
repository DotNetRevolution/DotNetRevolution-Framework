using DotNetRevolution.Core.Commanding.CodeContract;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

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

        public abstract ICommandHandlingResult Handle(TCommand command);

        public abstract Task<ICommandHandlingResult> HandleAsync(TCommand command);
        
        public ICommandHandlingResult Handle(ICommand command)
        {
            return Handle((TCommand) command);
        }

        public Task<ICommandHandlingResult> HandleAsync(ICommand command)
        {
            return HandleAsync((TCommand)command);
        }
    }
}
