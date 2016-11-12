using DotNetRevolution.Core.Projecting.CodeContract;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    [ContractClass(typeof(ProjectionFactoryContract))]
    public interface IProjectionFactory
    {
        IProjection GetProjection();
    }
}