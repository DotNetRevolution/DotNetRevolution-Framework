using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventStreamRevisionIdentity : ValueObject<EventStreamRevisionIdentity>
    {
        public Guid Value { get; }

        public EventStreamRevisionIdentity(Guid value)
        {
            Contract.Requires(value != Guid.Empty);

            Value = value;
        }

        public static implicit operator Guid(EventStreamRevisionIdentity identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator EventStreamRevisionIdentity(Guid guid)
        {
            Contract.Requires(guid != Guid.Empty);
            Contract.Ensures(Contract.Result<EventStreamRevisionIdentity>() != null);

            return new EventStreamRevisionIdentity(guid);
        }
    }
}
