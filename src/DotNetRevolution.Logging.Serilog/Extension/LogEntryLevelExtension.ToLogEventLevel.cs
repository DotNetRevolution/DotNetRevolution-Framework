using System;
using DotNetRevolution.Core.Logging;
using Serilog.Events;

namespace DotNetRevolution.Logging.Serilog.Extension
{
    public static partial class LogEntryLevelExtension
    {
        public static LogEventLevel ToLogEventLevel(this LogEntryLevel entryLevel)
        {
            switch (entryLevel)
            {
                case LogEntryLevel.Verbose:
                    return LogEventLevel.Verbose;

                case LogEntryLevel.Debug:
                    return LogEventLevel.Debug;
                    
                case LogEntryLevel.Information:
                    return LogEventLevel.Information;

                case LogEntryLevel.Warning:
                    return LogEventLevel.Warning;

                case LogEntryLevel.Error:
                    return LogEventLevel.Error;

                case LogEntryLevel.Fatal:
                    return LogEventLevel.Fatal;

                default:
                    throw new ArgumentOutOfRangeException("entryLevel");
            }
        }
    }
}