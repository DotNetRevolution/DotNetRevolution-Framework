namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProjection<TProjectionState> : IProjection
    {
        void Project(IProjectionContext<ID> context);
    }
}
