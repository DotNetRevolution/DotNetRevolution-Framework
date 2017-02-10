namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionHandlerFactory
    {
        IProjectionHandler GetHandler(IProjectionHandlerEntry entry);
    }
}
