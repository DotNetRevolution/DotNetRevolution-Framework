using DotNetRevolution.Core.Extension;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(AbstractProjectionManagerContract))]
    public abstract class ProjectionManager : IProjectionManager
    {
        private readonly IProjectionFactory _projectionFactory;

        protected ProjectionManager(IProjectionFactory projectionFactory)
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
