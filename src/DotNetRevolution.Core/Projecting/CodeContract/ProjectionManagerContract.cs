using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionManager))]
    internal abstract class ProjectionManagerContract : IProjectionManager
    {
        public void Project(IEnumerable<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            throw new NotImplementedException();
        }

        public void Wait(Guid domainEventId)
        {
            throw new NotImplementedException();
        }

        public void Wait(Guid domainEventId, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task WaitAsync(Guid domainEventId)
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(Guid domainEventId, TimeSpan timeout)
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
