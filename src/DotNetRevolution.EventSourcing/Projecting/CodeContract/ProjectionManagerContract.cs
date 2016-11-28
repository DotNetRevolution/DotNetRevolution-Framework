using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting.CodeContract
{
    [ContractClassFor(typeof(IProjectionManager))]
    internal abstract class ProjectionManagerContract : IProjectionManager
    {
        public void Project(EventProviderTransaction transaction)
        {
            Contract.Requires(transaction != null);

            throw new NotImplementedException();
        }

        public void Wait(Guid domainEventId)
        {
            Contract.Requires(domainEventId != Guid.Empty);

            throw new NotImplementedException();
        }

        public void Wait(Guid domainEventId, TimeSpan timeout)
        {
            Contract.Requires(domainEventId != Guid.Empty);

            throw new NotImplementedException();
        }

        public Task WaitAsync(Guid domainEventId)
        {
            Contract.Requires(domainEventId != Guid.Empty);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }

        public Task WaitAsync(Guid domainEventId, TimeSpan timeout)
        {
            Contract.Requires(domainEventId != Guid.Empty);
            Contract.Ensures(Contract.Result<Task>() != null);

            throw new NotImplementedException();
        }
    }
}
