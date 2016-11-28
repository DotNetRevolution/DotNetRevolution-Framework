using System;
using System.Threading.Tasks;
using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionManagerContract))]
    public interface IProjectionManager
    {
        void Project(EventProviderTransaction transaction);

        void Wait(Guid domainEventId);
        void Wait(Guid domainEventId, TimeSpan timeout);
        Task WaitAsync(Guid domainEventId);
        Task WaitAsync(Guid domainEventId, TimeSpan timeout);
    }
}