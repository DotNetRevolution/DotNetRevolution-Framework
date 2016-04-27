using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging;
using Shuttle.ESB.Core;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class SendMessageHandler<TMessage> : MessageHandler<TMessage> 
        where TMessage : IMessage
    {
        private readonly IServiceBus _bus;

        public SendMessageHandler(IServiceBus bus)
        {
            Contract.Requires(bus != null);

            _bus = bus;
        }

        public override void Handle(TMessage message, string correlationId)
        {
            _bus.Send(message, configurator => configurator.WithCorrelationId(correlationId));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_bus != null);
        }
    }
}
