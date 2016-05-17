using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class SqlEventStoreTests
    {
        private IEventStore _eventStore;        
        private IEventStreamProcessor _eventStreamProcessor = new EventStreamProcessor();

        [TestInitialize]
        public void Init()
        {         
            _eventStore = new SqlEventStore(
                new EventStreamProcessorProvider(
                    _eventStreamProcessor),
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
                new EventProvider(new EventProviderType(typeof(AccountAggregateRoot)),
                    domainEvents.AggregateRoot.Identity,
                    EventProviderVersion.Initial,
                    new EventProviderDescriptor(domainEvents.AggregateRoot.Identity.Value.ToString()),
                    new EventStream(domainEvents),
                    _eventStreamProcessor)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);
        }

        [TestMethod]
        public void CanCommit1000Transactions()
        {
            for (var i = 0; i < 1000; i++)
            {
                var command = new Create(100);
                var domainEvents = AccountAggregateRoot.Create(100);

                _eventStore.Commit(new Transaction("UnitTester",
                    command,
                    new EventProvider(new EventProviderType(typeof(AccountAggregateRoot)),
                        domainEvents.AggregateRoot.Identity,
                        EventProviderVersion.Initial,
                        new EventProviderDescriptor(domainEvents.AggregateRoot.Identity.Value.ToString()),
                        new EventStream(domainEvents),
                        _eventStreamProcessor)));

                //var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

                //Assert.IsNotNull(eventProvider);
            }
        }
    }
}
