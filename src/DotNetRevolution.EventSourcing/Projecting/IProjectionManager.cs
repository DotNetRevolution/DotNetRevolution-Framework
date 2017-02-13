using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionManagerContract))]
    public interface IProjectionManager
    {
        void Handle(IEventProvider eventProvider, params IProjectionContext[] projectionContexts);
    }
}