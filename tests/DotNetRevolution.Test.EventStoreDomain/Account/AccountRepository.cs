using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.AggregateRoot;

namespace DotNetRevolution.Test.EventStoreDomain.Account
{
    public class AccountRepository : EventProviderRepository<AccountAggregateRoot>
    {
        public AccountRepository(IEventStore eventStore) 
            : base(eventStore, 
                   new OnConventionAggregateRootProcessor())
        {
        }
    }
}
