using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjection))]
    internal abstract class ProjectionContract : IProjection
    {
        public ProjectionIdentity ProjectionIdentity
        {
            get
            {
                Contract.Ensures(Contract.Result<ProjectionIdentity>() != null);

                throw new NotImplementedException();
            }
        }

        public void Project(IProjectionContext context)
        {
            Contract.Requires(context != null);

            throw new NotImplementedException();
        }        
    }
}
