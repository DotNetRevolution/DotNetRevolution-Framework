using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionFactory))]
    internal abstract class ProjectionFactoryContract : IProjectionFactory
    {
        public IProjection GetProjection(IEventProvider eventProvider)
        {
            Contract.Requires(eventProvider != null);
            Contract.Ensures(Contract.Result<IProjection>() != null);

            throw new NotImplementedException();
        }
    }
}
