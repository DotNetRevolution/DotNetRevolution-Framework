using System.Collections.Generic;

namespace DotNetRevolution.Core.Domain
{
    public class NullDomainEventDispatcher : IDomainEventDispatcher
    {
        public void UseCatalog(IDomainEventCatalog catalog)
        {
            
        }

        public void Publish(object domainEvent)
        {
        }

        public void Publish(object domainEvent, string correlationId)
        {
        }

        public void PublishAll(IEnumerable<object> domainEvents)
        {
        }

        public void PublishAll(IEnumerable<object> domainEvents, string correlationId)
        {
        }
    }
}
