using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.EventSourcing;

namespace DotNetRevolution.Test.EventSourcing
{
    [TestClass]
    public class SingleMethodEventStreamProcessorTests
    {
        [TestMethod]
        public void CanGetAggregateRoot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            var eventProvider = new EventProvider<AccountAggregateRoot>(domainEvents, new SingleMethodEventStreamProcessor("Apply"));

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(domainEvents.AggregateRoot.Identity, aggregateRoot.Identity);
            Assert.AreEqual(command.BeginningBalance, aggregateRoot.Balance);
        }
    }
}
