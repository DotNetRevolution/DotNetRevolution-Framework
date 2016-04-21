using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;
using DotNetRevolution.Core.Logging;
using DotNetRevolution.Core.Sessions;
using DotNetRevolution.Logging.Serilog.Extension;
using Serilog.Core;
using Serilog.Events;
using DotNetRevolution.Core.Authentication;

namespace DotNetRevolution.Logging.Serilog
{
    public class LogEntryLevelManager : Disposable, ISerilogLogEntryLevelManager
    {
        private readonly ISessionManager _sessionManager;
        private readonly IIdentityManager _identityManager;
        
        private readonly Dictionary<string, LoggingLevelSwitch> _userLogSwitches = new Dictionary<string, LoggingLevelSwitch>();
        private readonly Dictionary<string, LoggingLevelSwitch> _sessionLogSwitches = new Dictionary<string, LoggingLevelSwitch>();

        private readonly LoggingLevelSwitch _applicationSwitch = new LoggingLevelSwitch();

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
                                    IIdentityManager identityManager,
                                    LogEntryLevel applicationLogEntryLevel)
        {
            Contract.Requires(sessionManager != null);
            Contract.Requires(identityManager != null);

            _sessionManager = sessionManager;
            _sessionManager.SessionRemoved += SessionRemoved;

            _identityManager = identityManager;
            
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
                _sessionManager.SessionRemoved -= SessionRemoved;
            }
        }

        private LoggingLevelSwitch FindLoggingLevelSwitch()
        {
            LoggingLevelSwitch logSwitch;

            if (FindSessionLoggingLevelSwitch(out logSwitch))
            {
                Contract.Assume(logSwitch != null);

                return logSwitch;
            }

            if (FindUserLoggingLevelSwitch(out logSwitch))
            {
                Contract.Assume(logSwitch != null);

                return logSwitch;
            }

            return null;
        }

        private bool FindUserLoggingLevelSwitch(out LoggingLevelSwitch logSwitch)
        {
            var identity = _identityManager.Current;

            var identityName = identity == null ? string.Empty : identity.Name;
            Contract.Assume(identityName != null);

            return _userLogSwitches.TryGetValue(identityName, out logSwitch);
        }

        private bool FindSessionLoggingLevelSwitch(out LoggingLevelSwitch logSwitch)
        {
            var session = _sessionManager.Current;

            var sessionId = session == null ? string.Empty : session.Id;
            
            return _sessionLogSwitches.TryGetValue(sessionId, out logSwitch);            
        }

        private void SessionRemoved(object sender, SessionEventArgs e)
        {
            Contract.Requires(e?.Session != null);
            
            _sessionLogSwitches.Remove(e.Session.Id);
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
            Contract.Invariant(_identityManager != null);
            Contract.Invariant(_applicationSwitch != null); 
        }
    }
}
