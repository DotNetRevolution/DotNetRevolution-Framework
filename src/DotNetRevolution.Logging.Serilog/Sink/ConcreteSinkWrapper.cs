using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class ConcreteSinkWrapper : SinkWrapper
    {
        private readonly ILogEventSink _sink;

        public ConcreteSinkWrapper(LogEventLevel logEventLevel, ILogEventSink sink) 
            : base(logEventLevel)
        {
            Contract.Requires(sink != null);

            _sink = sink;
        }

        public override void SetSink(LoggerSinkConfiguration configuration)
        {
            configuration.Sink(_sink, LogEventLevel);
        }
    }
}
