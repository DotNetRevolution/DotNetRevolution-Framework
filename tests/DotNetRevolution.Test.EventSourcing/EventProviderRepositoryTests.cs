using DotNetRevolution.Core.Caching;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Commanding.Domain;
using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
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
        protected Core.Commanding.Domain.IRepository<AccountAggregateRoot> Repository { get; set; }

        public virtual void CanGetAggregateRoot()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
        }

        public virtual async Task CanGetAggregateRootAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);
            
            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            await Repository.CommitAsync(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = await Repository.GetByIdentityAsync(domainEvents.AggregateRoot.Identity);
            
            Assert.IsNotNull(account);
        }

        public virtual void CanAddMultipleDomainEventsToSingleEventProvider()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);

            var ch = new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository);

            for (var i = 0; i < 50; i++)
            {
                ch.Handle(new Deposit(account.Identity, i));
            }
        }

        public virtual void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyException()
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), 100);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);

            var dispatcher = new CommandDispatcher(new CommandHandlerFactory(new CommandCatalog(new List<ICommandEntry> { new StaticCommandEntry(typeof(Deposit), new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository)) })));
            
            Parallel.For(0, 50, (i) =>
            {
                dispatcher.Dispatch(new Deposit(account.Identity, i));
            });
        }

        public async Task CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyExceptionAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            await Repository.CommitAsync(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = await Repository.GetByIdentityAsync(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);

            var dispatcher = new CommandDispatcher(new CommandHandlerFactory(new CommandCatalog(new List<ICommandEntry> { new StaticCommandEntry(typeof(Deposit), new AggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository)) })));

            var tasks = new Task[50];

            for (var j = 0; j < 50; j++)
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

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
            var ch = new SynchronizedAggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository, new AggregateRootSynchronizer(new AggregateRootSynchronizationCache()), new AggregateRootCache());

            Parallel.For(0, 50, (i) =>
            {
                ch.Handle(new Deposit(account.Identity, i));
            });
        }
        

        public virtual void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyAsync(int i)
        {
            var command = new Create(SequentialAtEndGuidGenerator.NewGuid(), i);
            var domainEvents = AccountAggregateRoot.Create(command);

            domainEvents.AggregateRoot.State.InternalStateTracker = GetStateTracker(domainEvents);

            Repository.Commit(command, domainEvents.AggregateRoot as AccountAggregateRoot);

            var account = Repository.GetByIdentity(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(account);
            var ch = new SynchronizedAggregateRootCommandHandler<AccountAggregateRoot, Deposit>(Repository, new AggregateRootSynchronizer(new AggregateRootSynchronizationCache()), new AggregateRootCache());

            var tasks = new Task[50];

            for ( var j = 0; j < 50;  j++)
            {
                tasks[j] =  ch.HandleAsync(new Deposit(account.Identity, j));
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
