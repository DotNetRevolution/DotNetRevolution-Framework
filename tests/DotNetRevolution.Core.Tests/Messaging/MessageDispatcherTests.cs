using DotNetRevolution.Core.Messaging;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Messaging
{
    [TestClass]
    public class MessageDispatcherTests
    {        
        private IMessageDispatcher _dispatcher;

        [TestInitialize]
        public void Init()
        {            
            var catalog = new MessageCatalog(new Collection<MessageEntry>
                {
                    new MessageEntry(typeof(Message1), typeof(MockMessageHandler<Message1>))
                });            
            
            _dispatcher = new MessageDispatcher(new MessageHandlerFactory(catalog));
        }

        [TestMethod]        
        public void CanDispatchRegisteredMessage()
        {
            _dispatcher.Dispatch(new Message1(Guid.NewGuid()));
        }

        [TestMethod]
        [ExpectedException(typeof(MessageHandlingException))]
        public void CannotDispatchUnregisteredMessage()
        {
            _dispatcher.Dispatch(new Message2(Guid.NewGuid()));
        }
    }
}
