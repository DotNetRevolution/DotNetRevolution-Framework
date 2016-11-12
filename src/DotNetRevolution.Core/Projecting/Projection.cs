using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class Projection : IProjection
    {
        private static readonly MethodInfo _genericProjectMethod = typeof(IProject<>).GetMethod("Project");

        public ProjectionIdentity ProjectionIdentity { get; }

        protected Projection(ProjectionIdentity identity)
        {
            Contract.Requires(identity != null);

            ProjectionIdentity = identity;
        }

        public void Project(IDomainEvent domainEvent)
        {
            var projectMethod = _genericProjectMethod.MakeGenericMethod(new[] { domainEvent.GetType() });
            projectMethod.Invoke(this, new[] { domainEvent });
        }        
    }

    public abstract class Projection<TProjectionState> : Projection
    {
        public TProjectionState State { get; }
        
        public Projection(ProjectionIdentity identity, TProjectionState state)
            : base(identity)
        {
            Contract.Requires(identity != null);
            Contract.Requires(state != null);
            
            State = state;
        }        
    }    
}
