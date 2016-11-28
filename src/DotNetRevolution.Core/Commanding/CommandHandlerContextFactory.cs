using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Metadata;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class CommandHandlerContextFactory : ICommandHandlerContextFactory
    {
        private readonly IEnumerable<IMetaFactory> _metaFactories;

        public CommandHandlerContextFactory(IEnumerable<IMetaFactory> metaFactories)
        {
            Contract.Requires(metaFactories != null);

            _metaFactories = metaFactories;
        }

        public ICommandHandlerContext GetContext(ICommand command)
        {
            MetaCollection metadata = GetMetadata();

            return new CommandHandlerContext(command, metadata);
        }

        public ICommandHandlerContext<TCommand> GetContext<TCommand>(TCommand command) where TCommand : ICommand
        {
            MetaCollection metadata = GetMetadata();

            return new CommandHandlerContext<TCommand>(command, metadata);
        }

        private MetaCollection GetMetadata()
        {
            Contract.Ensures(Contract.Result<MetaCollection>() != null);

            var metadata = new MetaCollection();

            _metaFactories.ForEach(x => metadata.Add(x.GetMeta()));

            return metadata;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_metaFactories != null);
        }
    }
}
