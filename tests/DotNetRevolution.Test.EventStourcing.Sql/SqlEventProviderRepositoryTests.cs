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
        public void AddManyRecords()
        {
            Parallel.For(0, 100000, i =>
            {
                try
                {
                    base.CanGetAggregateRoot();
                }
                catch (Exception e)
                { Assert.Fail(e.ToString()); }
            });
        }

        protected override EventStreamStateTracker GetStateTracker(DomainEventCollection domainEvents)
        {
            return new EventStreamStateTracker(new EventStream(domainEvents));
        }
    }
}
