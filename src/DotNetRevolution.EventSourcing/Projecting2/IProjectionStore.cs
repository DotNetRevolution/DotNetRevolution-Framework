using System.Threading.Tasks;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionStore
    {
        void SetPosition(ProjectionInstance instance);

        Task SetPositionAsync(ProjectionInstance instance);

        ProjectionInstance GetPosition<TProjection>(EventProviderIdentity eventProviderIdentity) where TProjection : IProjection;

        Task<ProjectionInstance> GetPositionAsync<TProjection>(EventProviderIdentity eventProviderIdentity) where TProjection : IProjection;
    }
}
