using System;

namespace DotNetRevolution.Core.Commanding
{
    public interface ICommandHandlingResult
    {
        Guid CommandId { get; }
    }
}