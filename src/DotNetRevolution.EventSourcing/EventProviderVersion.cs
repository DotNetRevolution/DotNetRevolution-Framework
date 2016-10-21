using DotNetRevolution.Core.Base;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderVersion : ValueObject<EventProviderVersion>, IComparable<EventProviderVersion>
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

        public int CompareTo(EventProviderVersion other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Value.CompareTo(other.Value);
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
