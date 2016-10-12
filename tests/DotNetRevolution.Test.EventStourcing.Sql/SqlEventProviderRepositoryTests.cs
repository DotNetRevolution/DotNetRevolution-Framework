using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class SqlEventProviderRepositoryTests : EventProviderRepositoryTests
    {        
        [TestInitialize]
        public void Init()
        {
            GuidGenerator.Default = new SequentialAtEndGuidGenerator();
            var typeFactory = new DefaultTypeFactory(new MD5HashProvider(Encoding.UTF8));

            var hash = typeFactory.GetHash(typeof(Created));

            var eventStore = new SqlEventStore(
                new DefaultUsernameProvider("UnitTester"),
                new JsonSerializer(),
                typeFactory,
                Encoding.UTF8,
                ConfigurationManager.ConnectionStrings["SqlEventStore"].ConnectionString);

            var streamProcessor = new EventStreamProcessor<AccountAggregateRoot, AccountState>(new AggregateRootBuilder<AccountAggregateRoot, AccountState>(), new AggregateRootStateBuilder<AccountState>());

            Repository = new EventStoreRepository<AccountAggregateRoot, AccountState>(eventStore, streamProcessor);
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
            Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
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
            Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
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
            Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
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
            Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
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
            Parallel.For(0, 10000, i =>
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
            var tasks = new Task[10000];

            for (var i = 0; i < 10000; i++)
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

        protected override EventStreamStateTracker GetStateTracker(DomainEventCollection domainEvents)
        {
            return new EventStreamStateTracker(new EventStream(domainEvents));
        }
    }
}
