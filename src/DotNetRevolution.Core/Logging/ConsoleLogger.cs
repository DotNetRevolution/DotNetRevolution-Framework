using DotNetRevolution.Core.Properties;
using System;
using System.Globalization;

namespace DotNetRevolution.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public virtual void Log(LogEntryLevel logEntryLevel, string message)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, message);
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContextDictionary logEntryContext)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, string.Format(CultureInfo.CurrentCulture, "{0} [{1}]", message, logEntryContext));
        }

        public virtual void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, string.Format(CultureInfo.CurrentCulture, "{0}: {1}", message, exception));
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContextDictionary logEntryContext)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, string.Format(CultureInfo.CurrentCulture, "{0} [{1}]: {2}", message, logEntryContext, exception));
        }

        public virtual void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, exception);
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContextDictionary logEntryContext)
        {
            Console.WriteLine(Resources.LogEntryFormat, logEntryLevel, DateTime.Now, string.Format(CultureInfo.CurrentCulture, "[{0}]: {1}", logEntryContext, exception));
        }
    }
}
