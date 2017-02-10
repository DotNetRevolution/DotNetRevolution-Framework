namespace DotNetRevolution.Core.Domain
{
    public interface IApply<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        void Apply(TDomainEvent domainEvent);
    }
}
