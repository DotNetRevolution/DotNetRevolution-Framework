using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Messaging
{
    public class ProcessDomainEventMessage : Message
    {
        public IDomainEvent DomainEvent { get; }

        public ProcessDomainEventMessage(Guid messageId, IDomainEvent domainEvent)
            : base(messageId)
        {
            Contract.Requires(domainEvent != null);
            Contract.Requires(messageId != Guid.Empty);

            DomainEvent = domainEvent;
        }
    }
}
