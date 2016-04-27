namespace DotNetRevolution.Core.Messaging
{
    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        public virtual bool Reusable => true;

        public abstract void Handle(TMessage message, string correlationId);
        
        public void Handle(IMessage message, string correlationId)
        {
            Handle((TMessage) message, correlationId);
        }
    }
}
