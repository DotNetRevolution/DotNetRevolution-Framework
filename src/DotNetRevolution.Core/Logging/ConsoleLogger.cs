using System;

namespace DotNetRevolution.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        private const string MessageFormat = "{0}: {1}: {2}";

        public virtual void Log(LogEntryLevel logEntryLevel, string message)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, message);
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContext logEntryContext)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, string.Format("{0} [{1}]", message, logEntryContext));
        }

        public virtual void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, string.Format("{0}: {1}", message, exception));
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContext logEntryContext)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, string.Format("{0} [{1}]: {2}", message, logEntryContext, exception));
        }

        public virtual void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, exception);
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContext logEntryContext)
        {
            Console.WriteLine(MessageFormat, logEntryLevel, DateTime.Now, string.Format("[{0}]: {1}", logEntryContext, exception));
        }
    }
}