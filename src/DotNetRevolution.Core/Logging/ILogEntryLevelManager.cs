﻿using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Logging.CodeContract;

namespace DotNetRevolution.Core.Logging
{
    [ContractClass(typeof(LogEntryLevelManagerContract))]
    public interface ILogEntryLevelManager
    {
        LogEntryLevel LogEntryLevel { [Pure] get; }

        void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel);
        void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel, string context);
    }
}
