using DotNetRevolution.Core.Command;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Command
{
    [TestClass]
    public class CommandDispatcherTests
    {        
        private ICommandDispatcher _dispatcher;

        [TestInitialize]
        public void Init()
        {            
            var catalog = new CommandCatalog(new Collection<CommandEntry>
                {
                    new CommandEntry(typeof(Command1), typeof(MockCommandHandler<Command1>))
                });            
            
            _dispatcher = new CommandDispatcher(new CommandHandlerFactory(catalog));
        }

        [TestMethod]        
        public void CanDispatchRegisteredCommand()
        {
            _dispatcher.Dispatch(new Command1());
        }

        [TestMethod]
        [ExpectedException(typeof(CommandHandlingException))]
        public void CannotDispatchUnregisteredCommand()
        {
            _dispatcher.Dispatch(new Command2());
        }
    }
}
