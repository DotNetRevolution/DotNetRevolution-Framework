using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.AggregateRoot;
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
            AccountAggregateRoot account;

            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100, out account);

            var aggregateRoot = new OnConventionAggregateRootProcessor().Process<AccountAggregateRoot>(new EventStream(domainEvents));
            
            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(domainEvents.AggregateRoot.Identity, aggregateRoot.Identity);
            Assert.AreEqual(command.BeginningBalance, aggregateRoot.Balance);
        }
    }
}
