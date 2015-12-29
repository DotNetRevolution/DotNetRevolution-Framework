using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging.CodeContract;

namespace DotNetRevolution.Core.Messaging
{
    [ContractClass(typeof(MessageDispatcherContract))]
    public interface IMessageDispatcher
    {
        void Dispatch(object message);

        void Dispatch(object message, string correlationId);
    }
}