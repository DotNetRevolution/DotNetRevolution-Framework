namespace DotNetRevolution.Core.Domain
{
    public interface IRepository<TAggregateRoot> 
    {
        TAggregateRoot GetById(Identity identity);

        void Save(TAggregateRoot aggregateRoot);
    }
}
