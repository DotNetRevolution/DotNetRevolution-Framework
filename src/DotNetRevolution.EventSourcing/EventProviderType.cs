using DotNetRevolution.Core.Domain;
using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.EventSourcing
{
    public class EventProviderType : ValueObject<EventProviderType>
    {
        public Type Type { get; }

        public EventProviderType(Type eventProviderType)
        {            
            Contract.Requires(eventProviderType != null);

            Type = eventProviderType;
        }
    }
}
