using DotNetRevolution.Core.Messaging;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetRevolution.Core.Tests.Messaging
{
    [TestClass]
    public class MessageCatalogTests
    {
        private IMessageCatalog _catalog;

        [TestInitialize]
        public void Init()
        {
            _catalog = new MessageCatalog();
        }

        [TestMethod]
        public void CanAddEntry()
        {
            _catalog.Add(new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Message1)));
        }

        [TestMethod]
        public void CanAddEntriesForTwoDifferentMessages()
        {
            _catalog.Add(new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>)));
            _catalog.Add(new MessageEntry(typeof(Message2), typeof(MockMessageHandler<Message2>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Message1)));
            Assert.IsNotNull(_catalog.GetEntry(typeof(Message2)));
        }

        [TestMethod]
        public void CanAddEntriesUsingFluentApi()
        {
            _catalog.Add(new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>)))
                    .Add(new MessageEntry(typeof(Message2), typeof(MockMessageHandler<Message2>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Message1)));
            Assert.IsNotNull(_catalog.GetEntry(typeof(Message2)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddDuplicateEntry()
        {
            var entry = new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>));

            _catalog.Add(entry);
            _catalog.Add(entry);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddMoreThanOneEntryForSameTypeOfMessage()
        {
            var secondEntry = new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>));

            _catalog.Add(new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>)));
            _catalog.Add(secondEntry);
        }        
    }
}
