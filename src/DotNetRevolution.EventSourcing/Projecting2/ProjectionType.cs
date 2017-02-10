using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing.Projecting2
{
    public class ProjectionType : ValueObject<ProjectionType>
    {
        public Type Type { get; }

        public ProjectionType(Type projectionType)
        {
            Contract.Requires(projectionType != null);

            Type = projectionType;
        }
    }
}
