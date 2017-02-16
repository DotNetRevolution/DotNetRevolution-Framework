using DotNetRevolution.Core.Extension;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public class ProjectionManager : IProjectionManager
    {
        private readonly IProjectionFactory _projectionFactory;

        public ProjectionManager(IProjectionFactory projectionFactory)
        {
            Contract.Requires(projectionFactory != null);

            _projectionFactory = projectionFactory;
        }
        
        public void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts)
        {
            // get projection
            var projection = _projectionFactory.GetProjection(eventProvider);

            // project
            projectionContexts.ForEach(projection.Project);
        }
        
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionFactory != null);
        }
    }
}
