using System;

namespace DotNetRevolution.Core.Logging
{
    public class ColoredConsoleLogger : ConsoleLogger
    {
        public override void Log(LogEntryLevel logEntryLevel, string message)
        {
            using (SetConsoleColor(logEntryLevel))
            {
                base.Log(logEntryLevel, message);
            }
        }

        public override void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            using (SetConsoleColor(logEntryLevel))
            {
                base.Log(logEntryLevel, message, exception);
            }
        }

        public override void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            using (SetConsoleColor(logEntryLevel))
            {
                base.Log(logEntryLevel, exception);
            }
        }

        private static TemporaryConsoleColor SetConsoleColor(LogEntryLevel logEntryLevel)
        {
            switch (logEntryLevel)
            {
                case LogEntryLevel.Verbose:
                    return new TemporaryConsoleColor(ConsoleColor.Gray);

                case LogEntryLevel.Debug:
                    return new TemporaryConsoleColor(ConsoleColor.White);

                case LogEntryLevel.Information:
                    return new TemporaryConsoleColor(ConsoleColor.Green);

                case LogEntryLevel.Warning:
                    return new TemporaryConsoleColor(ConsoleColor.Yellow);

                case LogEntryLevel.Error:
                    return new TemporaryConsoleColor(ConsoleColor.Red);

                case LogEntryLevel.Fatal:
                    return new TemporaryConsoleColor(ConsoleColor.DarkRed);
                    
                default:
                    throw new ArgumentOutOfRangeException("logEntryLevel");
            }
        }
    }
}
