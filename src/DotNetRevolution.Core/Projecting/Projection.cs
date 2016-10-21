using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class Projection<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        private static readonly MethodInfo _genericProjectMethod = typeof(IProject<>).GetMethod("Project");

        public ProjectionIdentity ProjectionIdentity { get; }

        public Projection(ProjectionIdentity projectionIdentity)
        {
            Contract.Requires(projectionIdentity != null);

            ProjectionIdentity = projectionIdentity;
        }

        public void Project(IDomainEvent domainEvent)
        {
            Contract.Requires(domainEvent != null);

            var projectMethod = _genericProjectMethod.MakeGenericMethod(new[] { domainEvent.GetType() });
            projectMethod.Invoke(this, new[] { domainEvent });
        }

        public void Project(IReadOnlyCollection<IDomainEvent> domainEvents)
        {
            Contract.Requires(domainEvents != null);

            domainEvents.ForEach(Project);
        }        
    }    
}
