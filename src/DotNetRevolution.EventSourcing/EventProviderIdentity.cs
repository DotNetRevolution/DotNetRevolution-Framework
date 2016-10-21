using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderIdentity : ValueObject<EventProviderIdentity>
    {
        public Guid Value { get; }

        public EventProviderIdentity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static implicit operator Guid(EventProviderIdentity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator EventProviderIdentity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<EventProviderIdentity>() != null);

            return new EventProviderIdentity(guid);
        }
    }
}
