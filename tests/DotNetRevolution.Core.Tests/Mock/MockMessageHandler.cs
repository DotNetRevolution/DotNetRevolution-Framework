using DotNetRevolution.Core.Messaging;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockMessageHandler<TMessage> : MessageHandler<TMessage>
        where TMessage : class
    {
        public override bool Reusable
        {
            get
            {
                return false;
            }
        }

        public override void Handle(TMessage message, string correlationId)
        {
        }        
    }
}
