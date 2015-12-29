using System;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Logging.CodeContract;

namespace DotNetRevolution.Core.Logging
{
    [ContractClass(typeof(LoggerContract))]
    public interface ILogger
    {
        void Log(LogEntryLevel logEntryLevel, string message);
        void Log(LogEntryLevel logEntryLevel, string message, LogEntryContext logEntryContext);

        void Log(LogEntryLevel logEntryLevel, string message, Exception exception);
        void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContext logEntryContext);

        void Log(LogEntryLevel logEntryLevel, Exception exception);
        void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContext logEntryContext);
    }
}