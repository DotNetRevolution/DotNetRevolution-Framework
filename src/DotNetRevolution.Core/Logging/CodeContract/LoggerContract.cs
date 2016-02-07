using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILogger))]
    internal abstract class LoggerContract : ILogger
    {
        public void Log(LogEntryLevel logEntryLevel, string message)
        {
            Contract.Requires(message != null);

            throw new NotImplementedException();
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContextDictionary logEntryContext)
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

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContextDictionary logEntryContext)
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

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContextDictionary logEntryContext)
        {
            Contract.Requires(exception != null);
            Contract.Requires(logEntryContext != null);
            Contract.Requires(logEntryContext.Any());
            
            throw new NotImplementedException();
        }
    }
}
