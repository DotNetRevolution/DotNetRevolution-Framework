using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderDescriptor : ValueObject<EventProviderDescriptor>
    {
        public string Value { get; }

        public EventProviderDescriptor(string value)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(value) == false);

            Value = value;
        }
    }
}
