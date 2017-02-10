using DotNetRevolution.Logging.Serilog.Sink.CodeContract;
using Serilog.Configuration;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    [ContractClass(typeof(SinkWrapperContract))]
    public abstract class SinkWrapper
    {
        public LogEventLevel LogEventLevel { get; }

        protected SinkWrapper(LogEventLevel logEventLevel)
        {
            LogEventLevel = logEventLevel;
        }

        public abstract void SetSink(LoggerSinkConfiguration configuration);
    }
}
