using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Messaging;
using Shuttle.Esb;

namespace DotNetRevolution.ShuttleESB.Messaging
{
    public class PublishMessageHandler<TMessage> : MessageHandler<TMessage>
        where TMessage : IMessage
    {
        private readonly IServiceBus _bus;

        public PublishMessageHandler(IServiceBus bus)
        {
            Contract.Requires(bus != null);

            _bus = bus;
        }

        public override void Handle(TMessage message, string correlationId)
        {
            _bus.Publish(message, configurator => configurator.WithCorrelationId(correlationId));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_bus != null);
        }
    }
}
