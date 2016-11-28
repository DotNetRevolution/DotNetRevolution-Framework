using DotNetRevolution.Core.Metadata;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerContext : IDomainEventHandlerContext
    {
        public MetaCollection Metadata { get; }

        public IDomainEvent DomainEvent { get; }

        public DomainEventHandlerContext(IDomainEvent domainEvent)
            : this(domainEvent, new MetaCollection())
        {
            Contract.Requires(domainEvent != null);
        }

        public DomainEventHandlerContext(IDomainEvent domainEvent, MetaCollection metadata)
        {
            Contract.Requires(domainEvent != null);
            Contract.Requires(metadata != null);

            Metadata = metadata;
            DomainEvent = domainEvent;
        }
    }
}
