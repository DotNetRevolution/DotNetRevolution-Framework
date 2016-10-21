using DotNetRevolution.Core.Messaging.CodeContract;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageContract))]
    public interface IMessage
    {
        Guid MessageId { [Pure] get; }
    }
}
