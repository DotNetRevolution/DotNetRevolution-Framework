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
                new EventProvider(domainEvents, _eventStreamProcessor)));

            var eventProvider = _eventStore.GetEventProvider<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            Assert.IsNotNull(eventProvider);
        }

        [TestMethod]
        public void CanCommitMultipleEventProvidersUsingOneTransaction()
        {
            _eventStore.Commit(new Transaction("UnitTester",
                new Create(100),
                new EventProvider(AccountAggregateRoot.Create(100), _eventStreamProcessor),
                new EventProvider(AccountAggregateRoot.Create(100), _eventStreamProcessor)));            
        }        
    }
}
