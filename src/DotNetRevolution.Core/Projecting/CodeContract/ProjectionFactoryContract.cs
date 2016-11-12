using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionFactory))]
    internal abstract class ProjectionFactoryContract : IProjectionFactory
    {
        public IProjection GetProjection()
        {
            Contract.Ensures(Contract.Result<IProjection>() != null);

            throw new NotImplementedException();
        }
    }
}
