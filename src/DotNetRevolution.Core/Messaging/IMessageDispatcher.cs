using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageDispatcherContract))]
    public interface IMessageDispatcher
    {
        void Dispatch(IMessage message);

        void Dispatch(IMessage message, string correlationId);
    }
}
