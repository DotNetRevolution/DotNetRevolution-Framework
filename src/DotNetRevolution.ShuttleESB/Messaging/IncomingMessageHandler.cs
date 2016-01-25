using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging;
using Shuttle.ESB.Core;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class IncomingMessageHandler<TMessage> : Shuttle.ESB.Core.IMessageHandler<TMessage> 
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

            _dispatcher.Dispatch(context.Message);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
