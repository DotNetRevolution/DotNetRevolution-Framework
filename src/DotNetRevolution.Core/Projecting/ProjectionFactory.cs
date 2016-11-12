using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class ProjectionFactory : IProjectionFactory
    {
        private readonly Type _projectionType;

        public ProjectionFactory(Type projectionType)
        {
            Contract.Requires(projectionType != null);

            _projectionType = projectionType;
        }

        public IProjection GetProjection()
        {
            return (IProjection)Activator.CreateInstance(_projectionType);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_projectionType != null);
        }
    }
}
