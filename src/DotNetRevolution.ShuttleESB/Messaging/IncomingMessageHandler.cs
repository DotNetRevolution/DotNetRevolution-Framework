using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging;
using Shuttle.Esb;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class IncomingMessageHandler<TMessage> : Shuttle.Esb.IMessageHandler<TMessage> 
        where TMessage : class
    {
        private readonly IMessageDispatcher _dispatcher;

        public bool IsReusable
        {
            get { return true; }
        }

        public IncomingMessageHandler(IMessageDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            _dispatcher = dispatcher;
        }

        public void ProcessMessage(IHandlerContext<TMessage> context)
        {
            Contract.Assume(context?.Message != null);

            _dispatcher.Dispatch((IMessage) context.Message);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
