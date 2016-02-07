using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Logging;
using DotNetRevolution.Core.Sessions;
using DotNetRevolution.Logging.Serilog.Extension;
using Serilog.Core;
using Serilog.Events;

namespace DotNetRevolution.Logging.Serilog
{
    public class LogEntryLevelManager : Disposable, ISerilogLogEntryLevelManager
    {
        private readonly ISessionManager _sessionManager;

        private readonly Dictionary<string, LoggingLevelSwitch> _userLogSwitches;
        private readonly Dictionary<string, LoggingLevelSwitch> _sessionLogSwitches;

        private readonly LoggingLevelSwitch _applicationSwitch;

        public LogEntryLevel LogEntryLevel
        {
            get
            {
                var logSwitch = FindLoggingLevelSwitch();

                return logSwitch == null
                    ? _applicationSwitch.MinimumLevel.ToLogEntryLevel()
                    : logSwitch.MinimumLevel.ToLogEntryLevel();
            }
        }

        public LoggingLevelSwitch LoggingLevelSwitch
        {
            get
            {
                var logSwitch = FindLoggingLevelSwitch();

                return logSwitch ?? _applicationSwitch;
            }
        }

        public LogEntryLevelManager(ISessionManager sessionManager,
                                    LogEntryLevel applicationLogEntryLevel)
        {
            Contract.Requires(sessionManager != null);

            _sessionManager = sessionManager;
            _sessionManager.SessionReleased += SessionReleased;

            _sessionLogSwitches = new Dictionary<string, LoggingLevelSwitch>();
            _userLogSwitches = new Dictionary<string, LoggingLevelSwitch>();
            _applicationSwitch = new LoggingLevelSwitch();

            SetLogEntryLevel(LogLevel.Application, applicationLogEntryLevel);
        }
        
        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Application:
                    _applicationSwitch.MinimumLevel = logEntryLevel.ToLogEventLevel();
                    break;

                case LogLevel.Session:
                    throw new InvalidOperationException("Use SetLogEntryLevel(LogLevel, LogEntryLevel, String) instead");
                    
                case LogLevel.User:
                    throw new InvalidOperationException("Use SetLogEntryLevel(LogLevel, LogEntryLevel, String) instead");
                    
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        public void SetLogEntryLevel(LogLevel logLevel, LogEntryLevel logEntryLevel, string context)
        {
            switch (logLevel)
            {
                case LogLevel.Application:
                    throw new InvalidOperationException("Use SetLogEntryLevel(LogLevel, LogEntryLevel) instead");

                case LogLevel.Session:
                    SetLogEntryLevel(_sessionLogSwitches, logEntryLevel.ToLogEventLevel(), context);
                    break;

                case LogLevel.User:
                    SetLogEntryLevel(_userLogSwitches, logEntryLevel.ToLogEventLevel(), context);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sessionManager.SessionReleased -= SessionReleased;
            }
        }

        private LoggingLevelSwitch FindLoggingLevelSwitch()
        {
            LoggingLevelSwitch logSwitch;

            var session = _sessionManager.Current;

            var sessionId = session == null ? string.Empty : session.Identity;
            Contract.Assume(sessionId != null);

            if (_sessionLogSwitches.TryGetValue(sessionId, out logSwitch))
            {
                Contract.Assume(logSwitch != null);

                return logSwitch;
            }

            return _userLogSwitches.TryGetValue(string.Empty, out logSwitch)
                       ? logSwitch
                       : null;
        }

        private void SessionReleased(object sender, SessionEventArgs e)
        {
            Contract.Requires(e?.Session?.Identity != null);

            _sessionLogSwitches.Remove(e.Session.Identity);
        }

        private static void SetLogEntryLevel(Dictionary<string, LoggingLevelSwitch> dictionary, LogEventLevel logEntryLevel, string context)
        {
            Contract.Requires(dictionary != null);
            Contract.Assume(context != null);

            LoggingLevelSwitch logSwitch;

            if (dictionary.TryGetValue(context, out logSwitch))
            {
                Contract.Assume(logSwitch != null);

                logSwitch.MinimumLevel = logEntryLevel;
            }
            else
            {
                dictionary[context] = new LoggingLevelSwitch(logEntryLevel);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_sessionManager != null);
            Contract.Invariant(_userLogSwitches != null);
            Contract.Invariant(_sessionLogSwitches != null);
            Contract.Invariant(_applicationSwitch != null);
        }
    }
}
