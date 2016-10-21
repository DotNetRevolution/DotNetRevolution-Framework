using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class AggregateRootIdentity : ValueObject<AggregateRootIdentity>
    {
        public Guid Value { get; }

        public AggregateRootIdentity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static implicit operator Guid(AggregateRootIdentity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator AggregateRootIdentity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<AggregateRootIdentity>() != null);

            return new AggregateRootIdentity(guid);
        }
    }
}
