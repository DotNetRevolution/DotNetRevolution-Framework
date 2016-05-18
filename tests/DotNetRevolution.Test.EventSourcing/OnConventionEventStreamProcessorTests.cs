using DotNetRevolution.EventSourcing;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Test.EventSourcing
{
    [TestClass]
    public class OnConventionEventStreamProcessorTests
    {
        [TestMethod]
        public void CanGetAggregateRoot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            var eventProvider = new EventProvider<AccountAggregateRoot>(domainEvents, new OnConventionEventStreamProcessor());

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(domainEvents.AggregateRoot.Identity, aggregateRoot.Identity);
            Assert.AreEqual(command.BeginningBalance, aggregateRoot.Balance);
        }
    }
}
