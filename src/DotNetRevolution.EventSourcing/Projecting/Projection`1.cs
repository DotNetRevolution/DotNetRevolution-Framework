using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public abstract class Projection<TProjectionState> : IProjection<TProjectionState>
    {
        public ProjectionIdentity ProjectionIdentity { get; }

        public TProjectionState State { get; }

        public Projection(ProjectionIdentity identity, TProjectionState state)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);

            ProjectionIdentity = identity;
            State = state;
        }

        public abstract void Project(IProjectionContext<TProjectionState> context);

        public void Project(IProjectionContext context)
        {
            var genericContext = context as IDomainEventHandlerContext<TDomainEvent>;

            if (genericContext == null)
            {
                Handle(new DomainEventHandlerContext<TDomainEvent>(context));
            }
            else
            {
                Handle(genericContext);
            }
        }
    }
}
