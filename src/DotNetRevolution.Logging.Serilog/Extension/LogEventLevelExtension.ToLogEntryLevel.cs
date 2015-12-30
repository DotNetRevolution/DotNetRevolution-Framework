using System;
using DotNetRevolution.Core.Logging;
using Serilog.Events;

namespace DotNetRevolution.Logging.Serilog.Extension
{
    public static partial class LogEventLevelExtension
    {
        public static LogEntryLevel ToLogEntryLevel(this LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return LogEntryLevel.Verbose;

                case LogEventLevel.Debug:
                    return LogEntryLevel.Debug;

                case LogEventLevel.Information:
                    return LogEntryLevel.Information;

                case LogEventLevel.Warning:
                    return LogEntryLevel.Warning;

                case LogEventLevel.Error:
                    return LogEntryLevel.Error;

                case LogEventLevel.Fatal:
                    return LogEntryLevel.Fatal;

                default:
                    throw new ArgumentOutOfRangeException("level");
            }
        }
    }
}