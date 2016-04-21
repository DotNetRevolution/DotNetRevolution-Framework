using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockReusableDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent>
        where TDomainEvent : class
    {
        public override void Handle(TDomainEvent domainEvent)
        {
        }
    }
}
