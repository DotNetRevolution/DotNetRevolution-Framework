using System;
using Serilog.Events;
using System.Diagnostics.Contracts;
using Serilog.Configuration;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class ConfigurationExtensionSinkWrapper : SinkWrapper
    {
        private readonly Action<LoggerSinkConfiguration> _sinkConfigurationAction;

        public ConfigurationExtensionSinkWrapper(LogEventLevel logEventLevel, Action<LoggerSinkConfiguration> sinkConfigurationAction) 
            : base(logEventLevel)
        {
            Contract.Requires(sinkConfigurationAction != null);

            _sinkConfigurationAction = sinkConfigurationAction;
        }

        public override void SetSink(LoggerSinkConfiguration configuration)
        {
            _sinkConfigurationAction(configuration);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_sinkConfigurationAction != null);
        }
    }
}
