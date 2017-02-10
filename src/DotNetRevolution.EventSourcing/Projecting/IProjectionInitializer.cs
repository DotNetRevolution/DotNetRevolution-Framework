using DotNetRevolution.Core.Domain;

namespace DotNetRevolution.EventSourcing.Projecting
{
    public interface IProjectionInitializer
    {
        void Initialize<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;

        void Initialize<TAggregateRoot>(int batchSize) where TAggregateRoot : class, IAggregateRoot;
    }
}
