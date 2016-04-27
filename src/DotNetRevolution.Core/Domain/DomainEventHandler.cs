namespace DotNetRevolution.Core.Domain
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public virtual bool Reusable
        {
            get
            {
                return true;
            }
        }

        public abstract void Handle(TDomainEvent domainEvent);

        public void Handle(IDomainEvent domainEvent)
        {
            Handle((TDomainEvent)domainEvent);
        }
    }
}
