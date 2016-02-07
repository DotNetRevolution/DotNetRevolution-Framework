using System;

namespace DotNetRevolution.Core.Logging
{
    public class EmptyLogger : ILogger
    {
        public void Log(LogEntryLevel logEntryLevel, string message)
        {
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContextDictionary logEntryContext)
        {
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContextDictionary logEntryContext)
        {
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContextDictionary logEntryContext)
        {
        }
    }
}
