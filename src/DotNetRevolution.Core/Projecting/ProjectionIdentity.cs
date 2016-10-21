using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Projecting
{
    public class ProjectionIdentity : ValueObject<ProjectionIdentity>
    {
        public Guid Value { get; }

        public ProjectionIdentity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static implicit operator Guid(ProjectionIdentity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator ProjectionIdentity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<ProjectionIdentity>() != null);

            return new ProjectionIdentity(guid);
        }
    }
}
