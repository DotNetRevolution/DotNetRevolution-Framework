using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessDomainEventMessage : Message
    {
        public IDomainEvent DomainEvent { get; }

        public ProcessDomainEventMessage(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            DomainEvent = domainEvent;
        }
    }
}
