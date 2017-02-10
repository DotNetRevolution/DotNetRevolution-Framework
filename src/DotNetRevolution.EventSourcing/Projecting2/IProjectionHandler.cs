using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public interface IProjectionDomainEventHandler<TProjection, TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent        
    {
        void Handle(IProjectionHandlerContext<TProjection, TDomainEvent> context);
    }

    public abstract class ProjectionDomainEventHandler<TProjection, TDomainEvent> : DomainEventHandler<TDomainEvent>, IProjectionDomainEventHandler<TProjection, TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public override void Handle(IDomainEventHandlerContext<TDomainEvent> context)
        {
            Handle((IProjectionHandlerContext<TProjection, TDomainEvent>) context);
        }

        public abstract void Handle(IProjectionHandlerContext<TProjection, TDomainEvent> context);
    }
}
