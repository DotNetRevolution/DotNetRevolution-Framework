using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProjectionContext<TDomainEvent> : IProjectionContext
    {
        [Pure]
        new TDomainEvent DomainEvent { get; }
    }
}
