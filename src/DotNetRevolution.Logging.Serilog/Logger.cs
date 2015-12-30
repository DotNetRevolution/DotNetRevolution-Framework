using System;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetRevolution.Core.Logging;
using ISeriLogger = Serilog.ILogger;

namespace DotNetRevolution.Logging.Serilog
{
    public class Logger : ILogger
    {
        public const string Template = "{Timestamp:G} [{Level}] {Message:l}";

        private readonly ISeriLogger _logger;

        public Logger(ISeriLogger logger)
        {
            Contract.Requires(logger != null, "logger");

            _logger = logger;
        }

        public void Log(LogEntryLevel logEntryLevel, string message)
        {
            WriteLogEntry(logEntryLevel, message, null, null);
        }

        public void Log(LogEntryLevel logEntryLevel, string message, LogEntryContext logEntryContext)
        {
            WriteLogEntry(logEntryLevel, message, null, logEntryContext);
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            WriteLogEntry(logEntryLevel, message, exception, null);
        }

        public void Log(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContext logEntryContext)
        {
            WriteLogEntry(logEntryLevel, message, exception, logEntryContext);
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            WriteLogEntry(logEntryLevel, string.Empty, exception, null);
        }

        public void Log(LogEntryLevel logEntryLevel, Exception exception, LogEntryContext logEntryContext)
        {
            WriteLogEntry(logEntryLevel, string.Empty, exception, logEntryContext);
        }

        private void WriteLogEntry(LogEntryLevel logEntryLevel, string message, Exception exception, LogEntryContext logEntryContext)
        {
            var logger = logEntryContext == null
                             ? _logger
                             : logEntryContext.Aggregate(_logger, (current, kvp) => current.ForContext(kvp.Key, kvp.Value));

            Contract.Assume(logger != null);

            if (exception == null)
            {
                switch (logEntryLevel)
                {
                    case LogEntryLevel.Verbose:
                        logger.Verbose(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Debug:
                        logger.Debug(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Information:
                        logger.Information(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Warning:
                        logger.Warning(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Error:
                        logger.Error(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Fatal:
                        logger.Fatal(Template, DateTime.Now, logEntryLevel, message);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("logEntryLevel");
                }
            }
            else
            {
                switch (logEntryLevel)
                {
                    case LogEntryLevel.Verbose:
                        logger.Verbose(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Debug:
                        logger.Debug(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Information:
                        logger.Information(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Warning:
                        logger.Warning(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Error:
                        logger.Error(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    case LogEntryLevel.Fatal:
                        logger.Fatal(exception, Template, DateTime.Now, logEntryLevel, message);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("logEntryLevel");
                }
            }
        }
    }
}