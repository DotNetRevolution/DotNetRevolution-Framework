using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.EventSourcing.Snapshotting;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Test.EventStoreDomain.Account.Snapshots;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class SqlEventStoreTests
    {
        private IEventStore _eventStore;
        private IAggregateRootProcessor _processor;
        private ISnapshotProvider _snapshotProvider;

        [TestInitialize]
        public void Init()
        {
            var snapshotProviderFactory = new SnapshotProviderFactory();
            snapshotProviderFactory.AddProvider(new EventProviderType(typeof(AccountAggregateRoot)), _snapshotProvider = new AccountSnapshotProvider());
            
            _eventStore = new SqlEventStore(
                new AggregateRootProcessorFactory(
                    _processor = new SingleMethodAggregateRootProcessor("Apply")),
                    new SnapshotPolicyFactory(new VersionSnapshotPolicy(1)),
                    snapshotProviderFactory,
                    new JsonSerializer(),
                    ConfigurationManager.ConnectionStrings["SqlEventStore"].ConnectionString);
        }

        [TestMethod]
        public void CanCommitTransactionAndGetEventProvider()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            _eventStore.Commit(new Transaction("UnitTester",
                command,
                new EventProvider(domainEvents)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);
        }

        [TestMethod]
        public void CanCommitMultipleEventProvidersUsingOneTransaction()
        {
            _eventStore.Commit(new Transaction("UnitTester",
                new Create(100),
                new EventProvider(AccountAggregateRoot.Create(100)),
                new EventProvider(AccountAggregateRoot.Create(100))));            
        }

        [TestMethod]
        public void CanCommitTransactionWithSnapshot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            _eventStore.Commit(new Transaction("UnitTester",
                command,
                new EventProvider<AccountAggregateRoot>(domainEvents, _processor, _snapshotProvider)));
        }

        [TestMethod]
        public void CanCommitTransactionAndGetEventProviderWithSnapshot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            _eventStore.Commit(new Transaction("UnitTester",
                command,
                new EventProvider<AccountAggregateRoot>(domainEvents, _processor, _snapshotProvider)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);
        }
    }
}
