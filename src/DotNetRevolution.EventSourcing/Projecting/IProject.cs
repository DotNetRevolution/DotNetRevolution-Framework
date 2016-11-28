using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProject<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        void Project(IProjectionContext<TDomainEvent> context);
    }
}
