using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.AggregateRoot;

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

            var aggregateRoot = new SingleMethodAggregateRootProcessor("Apply").Process<AccountAggregateRoot>(new EventStream(domainEvents));
            
            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(domainEvents.AggregateRoot.Identity, aggregateRoot.Identity);
            Assert.AreEqual(command.BeginningBalance, aggregateRoot.Balance);
        }
    }
}
