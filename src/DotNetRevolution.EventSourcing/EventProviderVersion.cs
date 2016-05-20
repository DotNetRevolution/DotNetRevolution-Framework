﻿using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderVersion : ValueObject<EventProviderVersion>
    {
        public static EventProviderVersion Initial = new EventProviderVersion(1);

        public int Value { [Pure] get; }

        public EventProviderVersion(int version)
        {
            Value = version;
        }

        [Pure]
        public EventProviderVersion Increment()
        {
            Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

            return new EventProviderVersion(Value + 1);
        }

        public static implicit operator int(EventProviderVersion identity)
        {
            Contract.Requires(identity != null);

            return identity.Value;
        }

        public static implicit operator EventProviderVersion(int value)
        {
            Contract.Ensures(Contract.Result<EventProviderVersion>() != null);

            return new EventProviderVersion(value);
        }
    }
}
