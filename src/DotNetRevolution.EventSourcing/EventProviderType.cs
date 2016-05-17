using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderType : ValueObject<EventProviderType>
    {
        public string FullName { get; }

        public EventProviderType(Type eventProviderType)
        {
            Contract.Requires(eventProviderType != null);

            FullName = eventProviderType.FullName;
        }
    }
}
