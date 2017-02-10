using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(ProjectionManager))]
    internal abstract class AbstractProjectionManagerContract : ProjectionManager
    {
        public AbstractProjectionManagerContract(IProjectionFactory projectionFactory) 
            : base(projectionFactory)
        {
            Contract.Requires(projectionFactory != null);
        }        
    }
}
