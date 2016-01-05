using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageDispatcher))]
    public abstract class MessageDispatcherContract : IMessageDispatcher
    {
        public void Dispatch(object message)
        {
            Contract.Requires(message != null);
        }

        public void Dispatch(object message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }
}
