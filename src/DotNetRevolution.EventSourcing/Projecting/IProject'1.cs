namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProject<in T>
    {
        void Project(IProjectionContext<T> context);
    }
}
