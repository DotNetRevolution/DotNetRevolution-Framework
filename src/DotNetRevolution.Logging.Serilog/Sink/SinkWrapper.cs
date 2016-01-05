using System.Diagnostics.Contracts;
using Serilog.Core;
using Serilog.Events;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class SinkWrapper
    {
        public ILogEventSink Sink { get; private set; }

        public LogEventLevel LogEventLevel { get; private set; }

        public SinkWrapper(ILogEventSink sink, LogEventLevel logEventLevel)
        {
            Contract.Requires(sink != null);

            Sink = sink;
            LogEventLevel = logEventLevel;
        }
    }
}
