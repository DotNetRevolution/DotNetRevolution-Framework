using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public abstract class Projection : IProjection
    {
        public ProjectionIdentity ProjectionIdentity { get; }

        protected Projection(ProjectionIdentity identity)
        {
            Contract.Requires(identity != null);

            ProjectionIdentity = identity;
        }

        public void Project(IProjectionContext context)
        {
            var projectType = typeof(IProject<>);            
            Contract.Assume(projectType.IsGenericTypeDefinition);

            var types = new Type[] { context.DomainEvent.GetType() };
            Contract.Assume(types.Length == projectType.GetGenericArguments().Length);

            var genericType = projectType.MakeGenericType(types);            

            var methodInfo = genericType.GetMethod("Project");
            Contract.Assume(methodInfo != null);

            methodInfo.Invoke(this, new object[] { domainEvent });            
        }        
    }  
}
