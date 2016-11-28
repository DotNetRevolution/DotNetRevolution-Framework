using DotNetRevolution.EventSourcing.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    [ContractClass(typeof(ProjectionContract))]
    public interface IProjection
    {
        ProjectionIdentity ProjectionIdentity { get; }
        
        void Project(IProjectionContext context);
    }
}