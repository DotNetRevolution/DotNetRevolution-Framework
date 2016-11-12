using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    [ContractClass(typeof(ProjectionContract))]
    public interface IProjection
    {
        ProjectionIdentity ProjectionIdentity { get; }
        
        void Project(IDomainEvent domainEvent);
    }
}