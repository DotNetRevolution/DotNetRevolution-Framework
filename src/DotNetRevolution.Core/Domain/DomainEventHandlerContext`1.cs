using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerContext<TDomainEvent> : DomainEventHandlerContext, IDomainEventHandlerContext<TDomainEvent> 
        where TDomainEvent : IDomainEvent
    {
        public new TDomainEvent DomainEvent { get; }

        public DomainEventHandlerContext(IDomainEventHandlerContext context)
            : this((TDomainEvent)context.DomainEvent, context.Metadata)
        {
            Contract.Requires(context != null);
        }

        public DomainEventHandlerContext(TDomainEvent domainEvent) 
            : this(domainEvent, new MetaCollection())
        {
            Contract.Requires(domainEvent != null);
        }

        public DomainEventHandlerContext(TDomainEvent domainEvent, MetaCollection metadata) 
            : base(domainEvent, metadata)
        {
            Contract.Requires(domainEvent != null);
            Contract.Requires(metadata != null);

            DomainEvent = domainEvent;
        }
    }
}
