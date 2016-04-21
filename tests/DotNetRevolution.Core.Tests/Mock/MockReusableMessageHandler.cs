using DotNetRevolution.Core.Messaging;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableMessageHandler<TMessage> : MessageHandler<TMessage>
        where TMessage : class
    {
        public override void Handle(TMessage message, string correlationId)
        {
        }
    }
}
