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

        public abstract void Handle(IDomainEventHandlerContext<TDomainEvent> context);

        public void Handle(IDomainEventHandlerContext context)
        {
            var genericContext = context as IDomainEventHandlerContext<TDomainEvent>;

            if (genericContext == null)
            {
                Handle(new DomainEventHandlerContext<TDomainEvent>(context));
            }
            else
            {
                Handle(genericContext);
            }
        }
    }
}
