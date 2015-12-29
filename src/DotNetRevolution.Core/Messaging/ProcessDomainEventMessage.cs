using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessDomainEventMessage
    {
        public object DomainEvent { get; }

        public ProcessDomainEventMessage(object domainEvent)
        {
            Contract.Requires(domainEvent != null);

            DomainEvent = domainEvent;
        }
    }
}