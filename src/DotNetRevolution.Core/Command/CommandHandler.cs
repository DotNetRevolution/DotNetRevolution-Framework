namespace DotNetRevolution.Core.Command
{
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class
    {
        public virtual bool Reusable
        {
            get { return true; }
        }

        public abstract void Handle(TCommand command);

        public void Handle(object command)
        {
            Handle((TCommand) command);
        }
    }
}
