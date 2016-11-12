using System;
using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting.CodeContract
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

        public void Project(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            throw new NotImplementedException();
        }

        public void Project(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            throw new NotImplementedException();
        }

        public void Project(params IDomainEvent[] domainEvents)
        {
            Contract.Requires(domainEvents != null);

            throw new NotImplementedException();
        }
    }
}
