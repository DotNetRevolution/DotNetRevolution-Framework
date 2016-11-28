using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandHandlerContext<TCommand> : CommandHandlerContext, ICommandHandlerContext<TCommand>
        where TCommand : ICommand
    {
        public new TCommand Command { get; }

        public CommandHandlerContext(ICommandHandlerContext context)
            : this((TCommand)context.Command, context.Metadata)
        {
            Contract.Requires(context != null);
        }

        public CommandHandlerContext(TCommand command) 
            : this(command, new MetaCollection())
        {
            Contract.Requires(command != null);
        }

        public CommandHandlerContext(TCommand command, MetaCollection metadata) 
            : base(command, metadata)
        {
            Contract.Requires(command != null);
            Contract.Requires(metadata != null);

            Command = command;
        }
    }
}
