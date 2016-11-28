using DotNetRevolution.Core.Extension;
using DotNetRevolution.Core.Metadata;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class DomainEventHandlerContextFactory : IDomainEventHandlerContextFactory
    {
        private readonly IEnumerable<IMetaFactory> _metaFactories;

        public DomainEventHandlerContextFactory(IEnumerable<IMetaFactory> metaFactories)
        {
            Contract.Requires(metaFactories != null);

            _metaFactories = metaFactories;
        }

        public IDomainEventHandlerContext GetContext(IDomainEvent domainEvent)
        {
            MetaCollection metadata = GetMetadata();

            return new DomainEventHandlerContext(domainEvent, metadata);
        }

        public IDomainEventHandlerContext<TDomainEvent> GetContext<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        {
            MetaCollection metadata = GetMetadata();

            return new DomainEventHandlerContext<TDomainEvent>(domainEvent, metadata);
        }

        private MetaCollection GetMetadata()
        {
            Contract.Ensures(Contract.Result<MetaCollection> () != null);

            var metadata = new MetaCollection();
            
            _metaFactories.ForEach(x => metadata.Add(x.GetMeta()));

            return metadata;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_metaFactories != null);
        }
    }
}
