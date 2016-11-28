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

        public abstract ICommandHandlingResult Handle(ICommandHandlerContext<TCommand> context);

        public abstract Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext<TCommand> context);
        
        public ICommandHandlingResult Handle(ICommandHandlerContext context)
        {
            var genericContext = context as ICommandHandlerContext<TCommand>;

            if (genericContext == null)
            {
                return Handle(new CommandHandlerContext<TCommand>(context));
            }
            else
            {
                return Handle(genericContext);
            }
        }

        public Task<ICommandHandlingResult> HandleAsync(ICommandHandlerContext context)
        {
            var genericContext = context as ICommandHandlerContext<TCommand>;

            if (genericContext == null)
            {
                return HandleAsync(new CommandHandlerContext<TCommand>(context));
            }
            else
            {
                return HandleAsync(genericContext);
            }
        }
    }
}
