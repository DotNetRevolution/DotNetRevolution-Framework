using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Commanding
{
    public class TransactionIdentity : ValueObject<TransactionIdentity>
    {
        public Guid Value { get; }

        public TransactionIdentity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static implicit operator Guid(TransactionIdentity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator TransactionIdentity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<TransactionIdentity>() != null);

            return new TransactionIdentity(guid);
        }
    }
}
