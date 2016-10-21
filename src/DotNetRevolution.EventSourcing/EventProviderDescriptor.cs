using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Domain;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderDescriptor : ValueObject<EventProviderDescriptor>
    {
        public string Value { get; }

        public EventProviderDescriptor(string value)
        {
            Value = value;
        }

        public EventProviderDescriptor(IAggregateRoot aggregateRoot)
        {
            Contract.Requires(aggregateRoot != null);

            Value = aggregateRoot.ToString();
        }
    }
}
