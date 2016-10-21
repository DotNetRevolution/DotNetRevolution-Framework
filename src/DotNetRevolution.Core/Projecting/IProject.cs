using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.Core.Projecting
{
    public interface IProject<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        void Project(TDomainEvent domainEvent);
    }
}
