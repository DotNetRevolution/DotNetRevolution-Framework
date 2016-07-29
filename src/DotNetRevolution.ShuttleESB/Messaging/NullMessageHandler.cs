using Shuttle.Esb;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class NullMessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : class
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessMessage(IHandlerContext<TMessage> context)
        {
        }
    }
}
