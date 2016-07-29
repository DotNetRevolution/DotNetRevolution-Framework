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
using DotNetRevolution.Test.EventStoreDomain.Account.Delegate;
using System.Threading.Tasks;
using System;
using DotNetRevolution.Core.Hashing;
using System.Text;

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
            var typeFactory = new DefaultTypeFactory(new MD5HashProvider(Encoding.UTF8));
            var snapshotProviderFactory = new SnapshotProviderFactory();
            snapshotProviderFactory.AddProvider(new EventProviderType(typeof(AccountAggregateRoot)), _snapshotProvider = new AccountSnapshotProvider());
            
            _eventStore = new SqlEventStore(
                new AggregateRootProcessorFactory(_processor = new SingleMethodAggregateRootProcessor("Apply")),
                new SnapshotPolicyFactory(new ExplicitVersionSnapshotPolicy(1)),
                snapshotProviderFactory,
                new JsonSerializer(),
                typeFactory,
                Encoding.UTF8,
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

        [TestMethod]
        public void AddManyRecords()
        {
            Parallel.For(0, 1000, i =>
             {
                 try
                 {
                     CanCommitTransactionAndGetEventProviderWithSnapshot();
                 }
                 catch (Exception e)
                 { Assert.Fail(e.ToString()); }
             });
        }

        [TestMethod]
        public void CanAppendEventsToExistingEventProviderWithSnapshot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            _eventStore.Commit(new Transaction("UnitTester",
                command,
                new EventProvider<AccountAggregateRoot>(domainEvents, _processor, _snapshotProvider)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);

            var command2 = new Deposit(domainEvents.AggregateRoot.Identity, 200);

            eventProvider = eventProvider.CreateNewVersion(aggregateRoot.Credit(command2.Amount, new CanCreditAccount(CanDepositAmount)));

            var newVersion = eventProvider.Version;

            _eventStore.Commit(new Transaction("UnitTester", command2, eventProvider));            
        }

        [TestMethod]
        public void CanAppendEventsToExistingEventProviderWithSnapshotAndGetAggregateRoot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            _eventStore.Commit(new Transaction("UnitTester",
                command,
                new EventProvider<AccountAggregateRoot>(domainEvents, _processor, _snapshotProvider)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);

            var command2 = new Deposit(domainEvents.AggregateRoot.Identity, 200);

            eventProvider = eventProvider.CreateNewVersion(aggregateRoot.Credit(command2.Amount, new CanCreditAccount(CanDepositAmount)));

            var newBalance = aggregateRoot.Balance;
            var newVersion = eventProvider.Version;

            _eventStore.Commit(new Transaction("UnitTester", command2, eventProvider));

            eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);
            Assert.IsTrue(eventProvider.Version == newVersion);

            aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(aggregateRoot.Balance, newBalance);
        }

        private bool CanDepositAmount(AccountAggregateRoot account, decimal amount, out string declinationReason)
        {
            declinationReason = string.Empty;

            return true;
        }
    }
}
