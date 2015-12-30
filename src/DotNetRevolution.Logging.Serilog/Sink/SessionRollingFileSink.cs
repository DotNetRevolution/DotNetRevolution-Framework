using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Session;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.RollingFile;

namespace DotNetRevolution.Logging.Serilog.Sink
{
    public class SessionRollingFileSink : ILogEventSink
    {
        private const int FileSize = 100000000;
        private const int NumberOfFilesToKeep = 10;
        private const string LogFormat = "{0}\\{1}-{2}.log";
        private const string LogDatePart = "{Date}";

        private readonly ISessionManager _sessionManager;
        private readonly string _logPath;
        private readonly Dictionary<string, RollingFileSink> _sinks;

        public SessionRollingFileSink(ISessionManager sessionManager,
                                      string logPath)
        {
            Contract.Requires(sessionManager != null);
            Contract.Requires(logPath != null);
            Contract.Requires(logPath.Length > 0);

            _sessionManager = sessionManager;
            _sessionManager.SessionReleased += SessionReleased;
            _logPath = logPath;

            _sinks = new Dictionary<string, RollingFileSink>();
        }
        
        public void Emit(LogEvent logEvent)
        {
            var sink = GetSink();
            
            sink.Emit(logEvent);
        }

        private RollingFileSink GetSink()
        {
            Contract.Ensures(Contract.Result<RollingFileSink>() != null);

            var session = _sessionManager.GetCurrentSession();

            var sessionId = session == null ? string.Empty : session.Identity;
            Contract.Assume(sessionId != null);

            RollingFileSink sink;

            if (_sinks.TryGetValue(sessionId, out sink))
            {
                Contract.Assume(sink != null);

                return sink;
            }

            sink = new RollingFileSink(string.Format(LogFormat, _logPath, LogDatePart, sessionId), new MessageTemplateTextFormatter(Logger.Template, null), FileSize, NumberOfFilesToKeep);

            _sinks[sessionId] = sink;

            return sink;
        }

        private void SessionReleased(object sender, ISession session)
        {
            Contract.Requires(session != null);
            Contract.Requires(session.Identity != null);

            RollingFileSink sink;

            if (_sinks.TryGetValue(session.Identity, out sink))
            {
                Contract.Assume(sink != null);

                sink.Dispose();
            }

            _sinks.Remove(session.Identity);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_sinks != null);
            Contract.Invariant(_sessionManager != null);
            Contract.Invariant(_logPath != null);
        }
    }
}