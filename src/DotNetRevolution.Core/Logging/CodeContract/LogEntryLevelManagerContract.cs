using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILogEntryLevelManager))]
    internal abstract class LogEntryLevelManagerContract : ILogEntryLevelManager
    {
        public LogEntryLevel LogEntryLevel
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel)
        {
            throw new NotImplementedException();
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel, string context)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(context));

            throw new NotImplementedException();
        }
    }
}
