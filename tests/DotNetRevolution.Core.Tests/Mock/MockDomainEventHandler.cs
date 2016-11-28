using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Tests.Mock
{
    public class MockDomainEventHandler<TDomainEvent> : DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public override bool Reusable
        {
            get
            {
                return false;
            }
        }

        public override void Handle(IDomainEventHandlerContext<TDomainEvent> context)
        {
        }        
    }
}
