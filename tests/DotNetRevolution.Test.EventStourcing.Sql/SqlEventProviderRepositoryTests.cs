using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.Core.Projecting;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using DotNetRevolution.Test.EventStoreDomain.Account.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class SqlEventProviderRepositoryTests : EventProviderRepositoryTests
    {
        private IGuidGenerator _guidGenerator;

        [TestInitialize]
        public void Init()
        {
            _guidGenerator = new SequentialAtEndGuidGenerator();

            var typeFactory = new DefaultTypeFactory(new MD5HashProvider(Encoding.UTF8));

            var hash = typeFactory.GetHash(typeof(Created));

            EventStore = new SqlEventStore(
                new DefaultUsernameProvider("UnitTester"),
                new JsonSerializer(),
                typeFactory,
                Encoding.UTF8,
                ConfigurationManager.ConnectionStrings["SqlEventStore"].ConnectionString);

            var streamProcessor = new EventStreamProcessor<AccountAggregateRoot, AccountState>(new AggregateRootBuilder<AccountAggregateRoot, AccountState>(), new AggregateRootStateBuilder<AccountState>());

            Repository = new EventStoreRepository<AccountAggregateRoot, AccountState>(EventStore, streamProcessor);
        }

        [TestMethod]
        public override void CanGetAggregateRoot()
        {
            base.CanGetAggregateRoot();
        }

        [TestMethod]
        public async Task CanGetAggregateRootAsync()
        {
            await base.CanGetAggregateRootAsync(100);
        }
        

        [TestMethod]
        public override void CanAddMultipleDomainEventsToSingleEventProvider()
        {
            Parallel.For(0, 50, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                try
                {
                    base.CanAddMultipleDomainEventsToSingleEventProvider();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.ToString());
                }
            });
        }

        [TestMethod]
        public override void CanAddMultipleDomainEventsToSingleEventProviderConcurrently()
        {
            Parallel.For(0, 50, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                try
                {
                    base.CanAddMultipleDomainEventsToSingleEventProviderConcurrently();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.ToString());
                }
            });
        }

        [TestMethod]
        public void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyAsync()
        {
            Parallel.For(0, 50, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                try
                {
                    base.CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyAsync(i);
                }
                catch (Exception e)
                {
                    Assert.Fail(e.ToString());
                }
            });
        }

        [TestMethod]
        public override void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyException()
        {
            Parallel.For(0, 20, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                try
                {
                    base.CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyException();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.ToString());
                }
            });
        }

        [TestMethod]
        public void CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyExceptionAsync()
        {
            var tasks = new Task[10];

            for (var i = 0; i < 10; i++)
            {
                tasks[i] = CanAddMultipleDomainEventsToSingleEventProviderConcurrentlyWithConcurrencyExceptionAsync(i);
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
        
        [TestMethod]
        public void AddManyRecords()
        {
            Parallel.For(0, 5000, i =>
            {
                try
                {
                    base.CanGetAggregateRoot();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.ToString());
                }
            });
        }

        [TestMethod]
        public void AddManyRecordsAsync()
        {
            var tasks = new Task[5000];

            for (var i = 0; i < 5000; i++)
            {
                tasks[i] = base.CanGetAggregateRootAsync(i);
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

        [TestMethod]
        public void Project()
        {
            var projection = new AccountProjection(new ProjectionIdentity(Guid.NewGuid()));
            var projectionManager = new MemoryProjectionManager<AccountProjection>(new MemoryProjectionFactory(projection));
            
            var projectionCatalog = new ProjectionCatalog();
            projectionCatalog.Add(new ProjectionEntry(typeof(AccountProjection), projectionManager));
            
            using (var projectionDomainDispatcher = new QueueDomainEventDispatcher(new ProjectionDomainEventDispatcher(new ProjectionManagerFactory(projectionCatalog))))
            {
                var announcer = new EventStoreTransactionAnnouncer(EventStore, projectionDomainDispatcher);

                Created createdDomainEvent = null;

                var domainEventCatalog = new DomainEventCatalog();
                domainEventCatalog.Add(new DomainEventEntry(typeof(Created), new ActionDomainEventHandler<Created>((created) => createdDomainEvent = created)));
                var announcer2 = new EventStoreTransactionAnnouncer(EventStore, new DomainEventDispatcher(new DomainEventHandlerFactory(domainEventCatalog)));

                base.CanGetAggregateRoot();

                projectionManager.Wait(createdDomainEvent.DomainEventId);

                Assert.AreEqual(projection.State.Accounts.Count, 1);
            }            
        }

        protected override EventStreamStateTracker GetStateTracker(DomainEventCollection domainEvents)
        {
            return new EventStreamStateTracker(new EventStream(_guidGenerator.Create(), domainEvents));
        }
    }
}
