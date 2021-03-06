﻿using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Metadata;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Tests.Commanding
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
            
            _dispatcher = new CommandDispatcher(new CommandHandlerFactory(catalog), new CommandHandlerContextFactory(new List<IMetaFactory>()));
        }

        [TestMethod]
        public void CanDispatchRegisteredCommand()
        {
            _dispatcher.Dispatch(new Command1());
        }

        [TestMethod]
        public void CanDispatchRegisteredCommandAsync()
        {
            Task.WaitAll(_dispatcher.DispatchAsync(new Command1()));
        }

        [TestMethod]
        [ExpectedException(typeof(CommandHandlingException))]
        public void CannotDispatchUnregisteredCommand()
        {
            _dispatcher.Dispatch(new Command2());
        }
    }
}
