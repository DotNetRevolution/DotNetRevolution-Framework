using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class MemoryProjectionFactory : IProjectionFactory
    {
        private readonly Dictionary<EventProviderIdentity, IProjection> _projections = new Dictionary<EventProviderIdentity, IProjection>();
        private readonly Type _projectionType;

        public MemoryProjectionFactory(Type projectionType)
        {
            Contract.Requires(projectionType != null);

            _projectionType = projectionType;
        }

        public IProjection GetProjection(IEventProvider eventProvider)
        {
            IProjection projection;

            if (_projections.TryGetValue(eventProvider.Identity, out projection))
            {
                Contract.Assume(projection != null);
                return projection;
            }

            projection = (IProjection)Activator.CreateInstance(_projectionType);

            _projections.Add(eventProvider.Identity, projection);

            return projection;
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projections != null);
            Contract.Invariant(_projectionType != null);
        }
    }
}
