using DotNetRevolution.Core.GuidGeneration;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Domain
{
    public class Identity : ValueObject<Identity>
    {
        public Guid Value { get; }

        public Identity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static Identity New()
        {
            Contract.Ensures(Contract.Result<Identity>() != null);

            return new Identity(GuidGenerator.Default.Create());
        }

        public static implicit operator Guid(Identity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator Identity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<Identity>() != null);
            
            return new Identity(guid);
        }
    }
}
