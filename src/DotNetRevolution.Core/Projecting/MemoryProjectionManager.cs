using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

namespace DotNetRevolution.Core.Projecting
{
    public class MemoryProjectionManager<TProjection> : ProjectionManager
        where TProjection : IProjection
    {
        private readonly HashSet<Guid> _processedDomainEvents = new HashSet<Guid>();

        public MemoryProjectionManager(IProjectionFactory projectionFactory) 
            : base(projectionFactory)
        {
            Contract.Requires(projectionFactory != null);
        }

        protected override void FinalizeProjection(IProjection projection)
        {
        }

        protected override void FinalizeProjection(IProjection projection, Exception e)
        {
        }

        protected override void PrepareProjection(IProjection projection)
        {
        }

        protected override bool Processed(Guid domainEventId)
        {
            return _processedDomainEvents.Contains(domainEventId);
        }

        protected override void SaveProjection(IProjection projection)
        {
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_processedDomainEvents != null);
        }
    }
}
