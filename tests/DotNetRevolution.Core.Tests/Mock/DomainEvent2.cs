using DotNetRevolution.Core.Domain;
using System;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class DomainEvent2 : IDomainEvent
    {
        public Guid DomainEventId { get; private set; }
    }
}
