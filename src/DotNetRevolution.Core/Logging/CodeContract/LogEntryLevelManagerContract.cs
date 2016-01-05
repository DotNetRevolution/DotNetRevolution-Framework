﻿using System;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Core.Logging.CodeContract
{
    [ContractClassFor(typeof(ILogEntryLevelManager))]
    public abstract class LogEntryLevelManagerContract : ILogEntryLevelManager
    {
        public LogEntryLevel GetLogEntryLevel()
        {
            throw new NotImplementedException();
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel)
        {
            throw new NotImplementedException();
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel, string context)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(context));

            throw new NotImplementedException();
        }
    }
}
