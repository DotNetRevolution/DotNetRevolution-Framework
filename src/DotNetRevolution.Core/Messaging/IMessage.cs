using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public interface IMessage
    {
        Guid Id { [Pure] get; }
    }
}
