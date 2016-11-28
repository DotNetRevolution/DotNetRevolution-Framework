using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IProjectionManagerFactory _projectionManagerFactory;

        public ProjectionDomainEventDispatcher(IProjectionManagerFactory projectionManagerFactory)
        {
            Contract.Requires(projectionManagerFactory != null);

            _projectionManagerFactory = projectionManagerFactory;
        }

        public void Publish(params IDomainEvent[] domainEvents)
        {
            // get managers
            var projectionManagers = _projectionManagerFactory.GetManagers();

            // loop through managers and project domain events
            foreach (var projectionManager in projectionManagers)
            {
                Contract.Assume(projectionManager != null);
                throw new System.Exception("SDF");
                // project
                //projectionManager.Project(domainEvents);
            }
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionManagerFactory != null);
        }
    }
}