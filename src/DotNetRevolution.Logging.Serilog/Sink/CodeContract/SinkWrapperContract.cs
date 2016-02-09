using Serilog.Events;
using System.Diagnostics.Contracts;
using Serilog.Configuration;

namespace DotNetRevolution.Logging.Serilog.Sink.CodeContract
{
    [ContractClassFor(typeof(SinkWrapper))]
    public abstract class SinkWrapperContract : SinkWrapper
    {
        public SinkWrapperContract(LogEventLevel logEventLevel) 
            : base(logEventLevel)
        {
        }

        public override void SetSink(LoggerSinkConfiguration configuration)
        {
            Contract.Requires(configuration != null);
        }
    }
}
