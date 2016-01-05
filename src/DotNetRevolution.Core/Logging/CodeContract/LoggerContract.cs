using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILogger))]
    public abstract class LoggerContract : ILogger
    {
        public void Log(LogEntryLevel logEntryLevel, string message)
        {
            Contract.Requires(message != null);

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContext logEntryContext)
        {
            Contract.Requires(message != null);
            Contract.Requires(logEntryContext != null);
            Contract.Requires(logEntryContext.Any());

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            Contract.Requires(message != null);
            Contract.Requires(exception != null);

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContext logEntryContext)
        {
            Contract.Requires(message != null);
            Contract.Requires(exception != null);
            Contract.Requires(logEntryContext != null);
            Contract.Requires(logEntryContext.Any());

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            Contract.Requires(exception != null);

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContext logEntryContext)
        {
            Contract.Requires(exception != null);
            Contract.Requires(logEntryContext != null);
            Contract.Requires(logEntryContext.Any());
            
            throw new NotImplementedException();
        }
    }
}
