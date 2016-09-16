using DotNetRevolution.Core.Extension;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public abstract class AggregateRootState : IAggregateRootState
    {
        public IStateTracker InternalStateTracker { get; set; }
        
        protected abstract void Redirect(object param);        

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
