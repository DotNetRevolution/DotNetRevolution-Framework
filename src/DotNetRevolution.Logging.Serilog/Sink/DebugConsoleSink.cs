using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class DebugConsoleSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            Contract.Assume(logEvent != null);

            Debug.WriteLine(logEvent.RenderMessage());
        }
    }
}
