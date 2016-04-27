using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageDispatcher))]
    internal abstract class MessageDispatcherContract : IMessageDispatcher
    {
        public void Dispatch(IMessage message)
        {
            Contract.Requires(message != null);
        }

        public void Dispatch(IMessage message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }
}
