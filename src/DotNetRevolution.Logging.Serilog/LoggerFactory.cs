using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using DotNetRevolution.Core.Logging;
using DotNetRevolution.Logging.Serilog.Sink;
using Serilog;
using Serilog.Core;
using ILogger = DotNetRevolution.Core.Logging.ILogger;

namespace DotNetRevolution.Logging.Serilog
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly ISerilogLogEntryLevelManager _logEntryLevelManager;
        private readonly SinkWrapper[] _sinkWrappers;
        private readonly ILogEventEnricher[] _enrichers;

        public LoggerFactory(ISerilogLogEntryLevelManager logEntryLevelManager,
                             SinkWrapper[] sinkWrappers,
                             ILogEventEnricher[] enrichers)
        {
            Contract.Requires(logEntryLevelManager != null);
            Contract.Requires(enrichers != null);
            Contract.Requires(sinkWrappers != null);
            Contract.Requires(sinkWrappers.Any());

            _logEntryLevelManager = logEntryLevelManager;
            _sinkWrappers = sinkWrappers;
            _enrichers = enrichers;
        }

        public ILogger Get(Type type)
        {
            var loggerConfiguration = new LoggerConfiguration();
            
            // set logging level runtime configurator
            SetLoggingLevelSwitch(loggerConfiguration);

            // add logging properties
            AddEnrichers(loggerConfiguration);

            // add all sinks
            AddSinks(loggerConfiguration);

            // create the logger with type context
            return CreateLogger(loggerConfiguration, type);
        }

        private Logger CreateLogger(LoggerConfiguration configuration, Type type)
        {
            Contract.Requires(configuration != null);
            Contract.Ensures(Contract.Result<Logger>() != null);

            var logger = configuration.CreateLogger();
            Contract.Assume(logger != null);

            logger.ForContext(type);

            return new Logger(logger);
        }

        private void SetLoggingLevelSwitch(LoggerConfiguration configuration)
        {
            Contract.Requires(configuration != null);
            
            var loggerMinimumLevelConfiguration = configuration.MinimumLevel;
            Contract.Assume(loggerMinimumLevelConfiguration != null);

            loggerMinimumLevelConfiguration.ControlledBy(_logEntryLevelManager.GetLoggingLevelSwitch());
        }

        private void AddEnrichers(LoggerConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            var loggerEnrichmentConfiguration = configuration.Enrich;
            Contract.Assume(loggerEnrichmentConfiguration != null);
            
            loggerEnrichmentConfiguration.WithMachineName();
            loggerEnrichmentConfiguration.WithProcessId();
            loggerEnrichmentConfiguration.With(_enrichers);
        }

        private void AddSinks(LoggerConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            Parallel.ForEach(_sinkWrappers, wrapper => configuration.WriteTo.Sink(wrapper.Sink, wrapper.LogEventLevel));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_logEntryLevelManager != null);
            Contract.Invariant(_sinkWrappers != null);
            Contract.Invariant(_enrichers != null);
        }
    }
}
