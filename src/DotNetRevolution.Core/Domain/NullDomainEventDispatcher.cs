using System.Collections.Generic;

namespace DotNetRevolution.Core.Domain
{
    public class NullDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Publish(object domainEvent)
        {
        }

        public void PublishAll(IEnumerable<object> domainEvents)
        {
        }
    }
}
