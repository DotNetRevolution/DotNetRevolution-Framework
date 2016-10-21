using System;
using System.Collections.Generic;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Extension;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class Projection
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

    public class AccountProjection : Projection,
        Core.Domain.IDomainEventHandler<Created>
    {
        public AccountProjection(ProjectionIdentity identity)
            : base(identity)
        {
            Contract.Requires(identity != null);

        }

        public bool Reusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Handle(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public void Handle(Created domainEvent)
        {
            throw new NotImplementedException();
        }
    }

    public class Created : Core.Domain.IDomainEvent
    {
        public Guid DomainEventId
        {
            get
            {
                return Guid.NewGuid();
            }
        }
    }
    public class Created2 : Core.Domain.IDomainEvent
    {
        public Guid DomainEventId
        {
            get
            {
                return Guid.NewGuid();
            }
        }
    }
}
