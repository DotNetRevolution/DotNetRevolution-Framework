namespace DotNetRevolution.Core.Domain
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class
    {
        public virtual bool Reusable
        {
            get
            {
                return true;
            }
        }

        public abstract void Handle(TDomainEvent domainEvent);

        public void Handle(object domainEvent)
        {
            Handle((TDomainEvent)domainEvent);
        }
    }
}
