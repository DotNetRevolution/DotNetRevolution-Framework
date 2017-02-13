using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionManager))]
    internal abstract class ProjectionManagerContract : IProjectionManager
    {
        public void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts)
        {
            Contract.Requires(eventProvider != null);
            Contract.Requires(projectionContexts != null);

            throw new NotImplementedException();
        }
    }
}
