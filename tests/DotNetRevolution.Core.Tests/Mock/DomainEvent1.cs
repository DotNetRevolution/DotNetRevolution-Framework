using System;
using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class DomainEvent1 : IDomainEvent
    {
        public Guid DomainEventId { get; private set; }
    }
}
