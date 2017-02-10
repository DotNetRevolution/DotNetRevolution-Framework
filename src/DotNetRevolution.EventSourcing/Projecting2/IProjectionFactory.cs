namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionFactory<TProjection>
    {
        TProjection GetProjection(IEventProvider eventProvider);
    }
}
