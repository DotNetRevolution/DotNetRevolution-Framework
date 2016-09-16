using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Test.EventStourcing
{
    public abstract class EventProviderRepositoryTests
    {
        protected Core.Commanding.IRepository<AccountAggregateRoot> Repository { get; set; }
        
        public virtual void CanGetAggregateRoot()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
        }

        protected abstract EventStreamStateTracker GetStateTracker(Core.Domain.DomainEventCollection domainEvents);
    }
}
