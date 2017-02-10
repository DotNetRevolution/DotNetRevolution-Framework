using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProjectionContext<out T> : IProjectionContext
    {
        [Pure]
        new T Data { get; }
    }
}
