using DotNetRevolution.Core.Domain;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.AggregateRoot;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRevolution.Test.EventSourcing
{
    [TestClass]
    public class MappedEventStreamProcessorTests
    {
        [TestMethod]
        public void CanGetAggregateRoot()
        {
            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100);

            var eventProvider = new EventProvider<AccountAggregateRoot>(Identity.New(), domainEvents, 
                new MappedEventStreamProcessor(new AggregateRootProcessorMap(typeof(Created), "Apply")));

            var aggregateRoot = eventProvider.CreateAggregateRoot();

            Assert.IsNotNull(aggregateRoot);
            Assert.AreEqual(domainEvents.AggregateRoot.Identity, aggregateRoot.Identity);
            Assert.AreEqual(command.BeginningBalance, aggregateRoot.Balance);
        }
    }
}
