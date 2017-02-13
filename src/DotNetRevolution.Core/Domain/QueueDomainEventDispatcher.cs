using DotNetRevolution.Core.Base;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class QueueDomainEventDispatcher : QueueDispatcher, IDomainEventDispatcher
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public QueueDomainEventDispatcher(IDomainEventDispatcher domainEventDispatcher)
        {
            Contract.Requires(domainEventDispatcher != null);

            _domainEventDispatcher = domainEventDispatcher;            
        }

        public void Publish(params IDomainEventHandlerContext[] contexts)
        {
            Queue.Add(new QueueItem<IDomainEventHandlerContext>(contexts));
        }
        
        public void Publish(params IDomainEvent[] domainEvents)
        {
            Queue.Add(new QueueItem<IDomainEvent>(domainEvents));
        }        
        
        protected override void Dispatch(QueueItem queueItem)
        {
            var domainEventQueueItem = queueItem as QueueItem<IDomainEvent>;

            if (domainEventQueueItem == null)
            {
                var contextItem = queueItem as QueueItem<IDomainEventHandlerContext>;
                Contract.Assume(contextItem != null);

                _domainEventDispatcher.Publish(contextItem.Items);
            }
            else
            {
                _domainEventDispatcher.Publish(domainEventQueueItem.Items);
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_domainEventDispatcher != null);
        }        
    }
}
