using System;

namespace DotNetRevolution.Core.Commanding
{
    public abstract class AggregateRootCommand : Command, IAggregateRootCommand
    {
        public Guid Identity { get; }

        public AggregateRootCommand(Guid identity)            
        {
            Identity = identity;
        }
    }
}
