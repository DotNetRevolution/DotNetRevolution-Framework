using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandHandlerContext : ICommandHandlerContext
    {
        public ICommand Command { get; }

        public MetaCollection Metadata { get; }

        public CommandHandlerContext(ICommand command)
            : this(command, new MetaCollection())
        {
            Contract.Requires(command != null);
        }

        public CommandHandlerContext(ICommand command, MetaCollection metadata)
        {
            Contract.Requires(command != null);
            Contract.Requires(metadata != null);

            Command = command;
            Metadata = metadata;
        }
    }
}
