using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetRevolution.Core.Tests.Domain
{
    [TestClass]
    public class DomainEventHandlerFactoryTests
    {
        private IDomainEventHandlerFactory _factory;

        [TestInitialize]
        public void Init()
        {
            var catalog = new DomainEventCatalog(new Collection<DomainEventEntry>
            {
                new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>)),
                new DomainEventEntry(typeof(DomainEvent2), typeof(MockReusableDomainEventHandler<DomainEvent2>))
            });

            _factory = new DomainEventHandlerFactory(catalog);
        }

        [TestMethod]
        public void HasCatalog()
        {
            Assert.IsTrue(_factory.Catalog != null);
        }

        [TestMethod]
        public void CanGetHandlerForRegisteredDomainEvent()
        {
            Assert.IsNotNull(_factory.GetHandlers(typeof(DomainEvent1)));
        }

        [TestMethod]
        public void CanGetCachedHandler()
        {
            var handlers = _factory.GetHandlers(typeof(DomainEvent2));
            var cachedHandlers = _factory.GetHandlers(typeof(DomainEvent2));

            Assert.AreEqual(handlers.Count, cachedHandlers.Count);
            Assert.AreEqual(handlers.DomainEventType, cachedHandlers.DomainEventType);

            Assert.IsTrue(handlers.Except(cachedHandlers).Count() == 0);
        }

        [TestMethod]
        public void AssertHandlerDoesNotCache()
        {
            var handler = _factory.GetHandlers(typeof(DomainEvent1));

            Assert.AreNotEqual(handler, _factory.GetHandlers(typeof(DomainEvent1)));
        }

        [TestMethod]        
        public void CannotGetHandlerForUnregisteredDomainEvent()
        {
            Assert.IsTrue(_factory.GetHandlers(typeof(object)).Count == 0);
        }
    }
}
