using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageHandlerContract))]
    public interface IMessageHandler
    {
        bool Reusable { [Pure] get; }
        void Handle(IMessage message, string correlationId);
    }
}
