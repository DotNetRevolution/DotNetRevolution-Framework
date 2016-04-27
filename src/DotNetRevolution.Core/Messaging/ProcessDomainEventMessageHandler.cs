using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessDomainEventMessageHandler : MessageHandler<ProcessDomainEventMessage>
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public ProcessDomainEventMessageHandler(IDomainEventDispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            _dispatcher = dispatcher;
        }

        public override void Handle(ProcessDomainEventMessage message, string correlationId)
        {
            Contract.Requires(message?.DomainEvent != null);

            _dispatcher.Publish(message.DomainEvent);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_dispatcher != null);
        }
    }
}
