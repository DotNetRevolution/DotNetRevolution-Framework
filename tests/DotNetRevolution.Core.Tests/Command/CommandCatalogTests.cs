using DotNetRevolution.Core.Command;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetRevolution.Core.Tests.Command
{
    [TestClass]
    public class CommandCatalogTests
    {
        private ICommandCatalog _catalog;

        [TestInitialize]
        public void Init()
        {
            _catalog = new CommandCatalog();
        }

        [TestMethod]
        public void CanAddEntry()
        {
            _catalog.Add(new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Command1)));
        }

        [TestMethod]
        public void CanAddEntriesForTwoDifferentCommands()
        {
            _catalog.Add(new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>)));
            _catalog.Add(new CommandEntry(typeof(Command2), typeof(MockCommandHandler<Command2>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Command1)));
            Assert.IsNotNull(_catalog.GetEntry(typeof(Command2)));            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddDuplicateEntry()
        {
            var entry = new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>));

            _catalog.Add(entry);
            _catalog.Add(entry);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddMoreThanOneEntryForSameTypeOfCommand()
        {
            var secondEntry = new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>));

            _catalog.Add(new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>)));
            _catalog.Add(secondEntry);
        }        
    }
}
