using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderVersion : ValueObject<EventProviderVersion>
    {
        public static EventProviderVersion Initial = new EventProviderVersion(0);

        public int Value { get; }

        public EventProviderVersion(int version)
        {
            Value = version;
        }

        public EventProviderVersion Increment()
        {
            return new EventProviderVersion(Value + 1);
        }
    }
}
