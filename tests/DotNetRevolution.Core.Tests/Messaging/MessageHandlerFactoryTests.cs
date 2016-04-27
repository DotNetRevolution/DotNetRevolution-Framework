using DotNetRevolution.Core.Messaging;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Messaging
{
    [TestClass]
    public class MessageHandlerFactoryTests
    {
        private IMessageHandlerFactory _factory;

        [TestInitialize]
        public void Init()
        {
            var catalog = new MessageCatalog(new Collection<MessageEntry>
            {
                new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>)),
                new MessageEntry(typeof(Message2), typeof(MockReusableMessageHandler<Message2>))
            });

            _factory = new MessageHandlerFactory(catalog);
        }

        [TestMethod]
        public void HasCatalog()
        {
            Assert.IsTrue(_factory.Catalog != null);
        }

        [TestMethod]
        public void CanGetHandlerForRegisteredMessage()
        {
            Assert.IsNotNull(_factory.GetHandler(typeof(Message1)));
        }

        [TestMethod]
        public void CanGetCachedHandler()
        {
            var handler = _factory.GetHandler(typeof(Message2));

            Assert.AreEqual(handler, _factory.GetHandler(typeof(Message2)));
        }

        [TestMethod]
        public void AssertHandlerDoesNotCache()
        {
            var handler = _factory.GetHandler(typeof(Message1));

            Assert.AreNotEqual(handler, _factory.GetHandler(typeof(Message1)));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CannotGetHandlerForUnregisteredMessage()
        {
            _factory.GetHandler(typeof(object));
        }
    }
}
