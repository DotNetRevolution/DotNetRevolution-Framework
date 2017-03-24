using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Logging
{
    public class ColoredConsoleLogger : ConsoleLogger
    {
        private static object ConsoleLock = new object();

        public override void Log(LogEntryLevel logEntryLevel, string message)
        {
            Log(logEntryLevel, () => base.Log(logEntryLevel, message));
        }
        
        public override void Log(LogEntryLevel logEntryLevel, string message, Exception exception)
        {
            Log(logEntryLevel, () => base.Log(logEntryLevel, message, exception));
        }

        public override void Log(LogEntryLevel logEntryLevel, Exception exception)
        {
            Log(logEntryLevel, () => base.Log(logEntryLevel, exception));
        }

        private void Log(LogEntryLevel logEntryLevel, Action action)
        {
            Contract.Requires(action != null);

            lock (ConsoleLock)
            {
                using (SetConsoleColor(logEntryLevel))
                {
                    action();
                }
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
