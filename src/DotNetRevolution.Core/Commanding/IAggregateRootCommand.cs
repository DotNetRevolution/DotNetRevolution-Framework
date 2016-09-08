using System;

namespace DotNetRevolution.Core.Commanding
{
    public interface IAggregateRootCommand : ICommand
    {
        Guid Identity { get; }
    }
}
