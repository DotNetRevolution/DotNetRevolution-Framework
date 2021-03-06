﻿using DotNetRevolution.Core.Logging;
using DotNetRevolution.Logging.Serilog;
using DotNetRevolution.Logging.Serilog.Sink;
using Ninject.Modules;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;

namespace DotNetRevolution.Ninject.Serilog
{
    public class SerilogModule : NinjectModule
    {        
        private readonly LogEventLevel _defaultMinimumLevel;

        public SerilogModule(string serilogLogFilePath,
                             LogEventLevel defaultMinimumLevel)
        {
            Contract.Requires(string.IsNullOrWhiteSpace(serilogLogFilePath) == false);

            _defaultMinimumLevel = defaultMinimumLevel;

            SetSerilogInternalLogger(serilogLogFilePath);
        }
        
        public override void Load()
        {
            // used to dynamically adjust the log entry level at runtime
            BindLogEntryLevelManager();

            // main logger factory
            BindMainLoggerFactory();

            // bind sink for all log factories
            BindDefaultSink();
        }

        private void BindMainLoggerFactory()
        {
            var binding = Bind<ILoggerFactory>();
            Contract.Assume(binding != null);

            var bindingTo = binding.To<LoggerFactory>();
            Contract.Assume(bindingTo != null);

            bindingTo.Named(LogFactoryName.Main.ToString());
        }

        private void BindLogEntryLevelManager()
        {
            var binding = Bind<ILogEntryLevelManager, ISerilogLogEntryLevelManager>();
            Contract.Assume(binding != null);

            var bindingTo = binding.To<LogEntryLevelManager>();
            Contract.Assume(bindingTo != null);

            bindingTo.WithConstructorArgument(typeof(LogEntryLevel), _defaultMinimumLevel);
        }

        [Conditional("DEBUG")]
        private void BindDefaultSink()
        {
            var binding = Bind<SinkWrapper>();
            Contract.Assume(binding != null);

            var bindingMethod = binding.ToConstant(new ConcreteSinkWrapper(_defaultMinimumLevel, new DebugConsoleSink()));
            Contract.Assume(bindingMethod != null);

            bindingMethod.WhenParentNamed(LogFactoryName.Main.ToString());
        }

        private static void SetSerilogInternalLogger(string serilogLogFilePath)
        {
            Contract.Requires(serilogLogFilePath != null);

            var path = Path.Combine(serilogLogFilePath, string.Format("Serilog-Error-{0}.log", Guid.NewGuid()));
            Contract.Assume(string.IsNullOrWhiteSpace(path) == false);

            // send Serilog errors to console
            SelfLog.Enable(new StreamWriter(path, true));
        }
    }
}
