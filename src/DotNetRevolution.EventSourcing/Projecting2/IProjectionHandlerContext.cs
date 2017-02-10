using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Metadata;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionHandlerContext
    {
        object Projection { get; }

        MetaCollection Metadata { get; }

        IDomainEvent DomainEvent { get; }
    }

    public interface IProjectionHandlerContext<TProjection, TDomainEvent> : IProjectionHandlerContext
        where TDomainEvent : IDomainEvent
    {
        new TProjection Projection { get; }

        new TDomainEvent DomainEvent { get; }
    }
}
