using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class AggregateRootCommand<TAggregateRoot> : Command, IAggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public Guid AggregateRootId { get; }

        public AggregateRootCommand(Guid commandId, Guid aggregateRootId)            
            : base(commandId)
        {
            Contract.Requires(commandId != Guid.Empty);
            
            AggregateRootId = aggregateRootId;
        }
    }
}
