using DotNetRevolution.Core.Command;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Command
{
    [TestClass]
    public class CommandHandlerFactoryTests
    {
        private ICommandHandlerFactory _factory;

        [TestInitialize]
        public void Init()
        {
            var catalog = new CommandCatalog(new Collection<CommandEntry>
            {
                new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>)),
                new CommandEntry(typeof(Command2), typeof(MockReusableCommandHandler<Command2>))
            });

            _factory = new CommandHandlerFactory(catalog);
        }

        [TestMethod]
        public void HasCatalog()
        {
            Assert.IsTrue(_factory.Catalog != null);
        }

        [TestMethod]
        public void CanGetHandlerForRegisteredCommand()
        {
            Assert.IsNotNull(_factory.GetHandler(typeof(Command1)));
        }

        [TestMethod]
        public void CanGetCachedHandler()
        {
            var handler = _factory.GetHandler(typeof(Command2));

            Assert.AreEqual(handler, _factory.GetHandler(typeof(Command2)));
        }

        [TestMethod]
        public void AssertHandlerDoesNotCache()
        {
            var handler = _factory.GetHandler(typeof(Command1));

            Assert.AreNotEqual(handler, _factory.GetHandler(typeof(Command1)));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CannotGetHandlerForUnregisteredCommand()
        {
            _factory.GetHandler(typeof(object));
        }
    }
}
