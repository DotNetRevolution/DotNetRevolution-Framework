using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionFactoryContract))]
    public interface IProjectionFactory
    {
        IProjection GetProjection();
    }
}