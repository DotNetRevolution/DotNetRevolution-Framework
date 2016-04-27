using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging.CodeContract
{
    [ContractClassFor(typeof(IMessageHandler))]
    internal abstract class MessageHandlerContract : IMessageHandler
    {
        public abstract bool Reusable { get; }

        public void Handle(IMessage message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }

    [ContractClassFor(typeof(IMessageHandler<>))]
    internal abstract class MessageHandlerContract<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        public abstract bool Reusable { get; }

        public void Handle(IMessage message, string correlationId)
        {
        }

        public void Handle(TMessage message, string correlationId)
        {
            Contract.Requires(message != null);
        }
    }
}
