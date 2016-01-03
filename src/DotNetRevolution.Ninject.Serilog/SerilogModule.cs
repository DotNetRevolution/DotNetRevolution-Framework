using DotNetRevolution.Core.Logging;
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
        public const string MainLoggerFactoryName = "MainLoggerFactory";
        
        private readonly LogEventLevel _defaultMinimumLevel;

        public SerilogModule(string serilogLogFilePath,
                             LogEventLevel defaultMinimumLevel)
        {
            Contract.Requires(serilogLogFilePath != null);
            Contract.Requires(serilogLogFilePath.Length > 0);

            _defaultMinimumLevel = defaultMinimumLevel;

            // send Serilog errors to console
            SelfLog.Out = new StreamWriter(Path.Combine(serilogLogFilePath, string.Format("Serilog-Error-{0}.log", Guid.NewGuid())), true);
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

            bindingTo.Named(MainLoggerFactoryName);
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

            var bindingMethod = binding.ToMethod(context => new SinkWrapper(new DebugConsoleSink(), _defaultMinimumLevel));

            Contract.Assume(bindingMethod != null);

            bindingMethod.WhenParentNamed(MainLoggerFactoryName);
        }
    }
}