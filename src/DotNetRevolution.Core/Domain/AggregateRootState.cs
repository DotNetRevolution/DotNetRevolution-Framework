using DotNetRevolution.Core.Extension;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRootState : IAggregateRootState
    {
        public IStateTracker InternalStateTracker { get; set; }
        
        private AggregateRootState(object snapshot)
        {
            Contract.Requires(snapshot != null);

            Redirect(snapshot);            
        }
        
        protected AggregateRootState(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            Redirect(domainEvents);
        }

        private AggregateRootState(object snapshot, IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(snapshot != null);
            Contract.Requires(domainEvents != null);

            Redirect(snapshot);
            Redirect(domainEvents);
        }

        public abstract void Redirect(object param);        

        public void Apply(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            if (InternalStateTracker != null)
            {
                InternalStateTracker.Apply(domainEvents);
            }

            Redirect(domainEvents);
        }

        public void Apply(IDomainEvent domainEvent)
        {
            if (InternalStateTracker != null)
            {
                InternalStateTracker.Apply(domainEvent);
            }

            Redirect(domainEvent);
        }

        private void Redirect(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            domainEvents.ForEach(Redirect);
        }
    }
}
