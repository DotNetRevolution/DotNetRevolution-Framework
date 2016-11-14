using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace DotNetRevolution.Core.Projecting
{
    public abstract class Projection : IProjection
    {
        public ProjectionIdentity ProjectionIdentity { get; }

        protected Projection(ProjectionIdentity identity)
        {
            Contract.Requires(identity != null);

            ProjectionIdentity = identity;
        }

        public void Project(IDomainEvent domainEvent)
        {
            var projectType = typeof(IProject<>);            
            Contract.Assume(projectType.IsGenericTypeDefinition);

            var types = new Type[] { domainEvent.GetType() };
            Contract.Assume(types.Length == projectType.GetGenericArguments().Length);

            var genericType = projectType.MakeGenericType(types);            

            var methodInfo = genericType.GetMethod("Project");
            Contract.Assume(methodInfo != null);

            methodInfo.Invoke(this, new object[] { domainEvent });            
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
