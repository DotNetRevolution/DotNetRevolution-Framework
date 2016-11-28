using DotNetRevolution.Core.Caching;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Commanding.Domain;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.Core.Metadata;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStourcing
{
    public abstract class EventProviderRepositoryTests
    {
        public EventProviderRepositoryTests()
        {
            MetaFactories = new List<IMetaFactory>();
        }

        protected List<IMetaFactory> MetaFactories { get; set; }

        protected IEventStore EventStore { get; set; }

        protected Core.Commanding.Domain.IRepository<AccountAggregateRoot> Repository { get; set; }

        public virtual void CanGetAggregateRoot()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
        }

        public virtual async Task CanGetAggregateRootAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);
            
            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            await Repository.CommitAsync(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = await Repository.GetByIdentityAsync(domainEvents.AggregateRoot.Identity);
            
            Assert.IsNotNull(account);
        }

        public virtual void CanAddMultipleDomainEventsToSingleEventProvider()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);

            var ch = new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository);
            var ch1 = new AggregateRootCommandHandler<AccountAggregateRoot, Withdraw>(Repository);

            var random = new Random();

            for (var i = 0; i < 15; i++)
            {
                ch.Handle(new CommandHandlerContext(new Deposit(account.Identity, i)));
                ch1.Handle(new CommandHandlerContext(new Withdraw(account.Identity, random.Next(1, 10) * i)));
            }
        }

        public virtual void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyException()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
            
            var dispatcher = new CommandDispatcher(new CommandHandlerFactory(new CommandCatalog(new List<ICommandEntry> { new StaticCommandEntry(typeof(Deposit), new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository)) })), new CommandHandlerContextFactory(MetaFactories));
            
            Parallel.For(0, 30, (i) =>
            {
                dispatcher.Dispatch(new Deposit(account.Identity, i));
            });
        }

        public async Task CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyExceptionAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            await Repository.CommitAsync(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = await Repository.GetByIdentityAsync(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);

            var dispatcher = new CommandDispatcher(new CommandHandlerFactory(new CommandCatalog(new List<ICommandEntry> { new StaticCommandEntry(typeof(Deposit), new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository)) })), new CommandHandlerContextFactory(MetaFactories));

            var tasks = new Task[30];

            for (var j = 0; j < 30; j++)
            {
                tasks[j] = dispatcher.DispatchAsync(new Deposit(account.Identity, j));
            };

            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        public virtual void CanAddMultipleDomainEventsToSingleEventProviderConcurrently()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
            var synchronizer = new AggregateRootSynchronizer(new AggregateRootSynchronizationCache());
            var aggregateRootCache = new AggregateRootCache();

            var ch = new SynchronizedAggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository, synchronizer, aggregateRootCache);
            var ch1 = new SynchronizedAggregateRootCommandHandler<AccountAggregateRoot, Withdraw2>(Repository, synchronizer, aggregateRootCache);

            var random = new Random();

            Parallel.For(0, 20, (i) =>
            {
                ch.Handle(new CommandHandlerContext(new Deposit(account.Identity, i)));
                ch1.Handle(new CommandHandlerContext(new Withdraw2(account.Identity, random.Next(1, 10) * i)));
            });
        }
        

        public virtual void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.ExternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(new CommandHandlerContext(command), domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
            var ch = new SynchronizedAggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository, new AggregateRootSynchronizer(new AggregateRootSynchronizationCache()), new AggregateRootCache());

            var tasks = new Task[10];

            for ( var j = 0; j < 10;  j++)
            {
                tasks[j] = ch.HandleAsync(new CommandHandlerContext(new Deposit(account.Identity, j)));
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        protected abstract EventStreamStateTracker GetStateTracker(DomainEventCollection domainEvents);
    }
}
