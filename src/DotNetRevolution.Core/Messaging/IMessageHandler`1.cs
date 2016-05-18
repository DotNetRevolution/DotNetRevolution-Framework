using DotNetRevolution.Core.Messaging.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageHandlerContract<>))]
    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        void Handle(TMessage message, string correlationId);
    }
}
