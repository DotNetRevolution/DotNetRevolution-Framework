using System;
using DotNetRevolution.Core.Logging;
using Serilog.Core;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.CodeContract
{
    [ContractClassFor(typeof(ISerilogLogEntryLevelManager))]
    internal abstract class SerilogLogEntryLevelManagerContract : ISerilogLogEntryLevelManager
    {
        public LogEntryLevel GetLogEntryLevel()
        {
            throw new NotImplementedException();
        }

        public LoggingLevelSwitch GetLoggingLevelSwitch()
        {
            Contract.Ensures(Contract.Result<LoggingLevelSwitch>() != null);

            throw new NotImplementedException();
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel)
        {
            throw new NotImplementedException();
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel, string context)
        {
            throw new NotImplementedException();
        }
    }
}
