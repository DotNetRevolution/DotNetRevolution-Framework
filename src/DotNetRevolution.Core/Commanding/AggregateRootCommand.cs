using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class AggregateRootCommand<TAggregateRoot> : Command, IAggregateRootCommand<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        public Guid Identity { get; }

        public AggregateRootCommand(Guid identity)            
        {
            Identity = identity;
        }
    }
}
