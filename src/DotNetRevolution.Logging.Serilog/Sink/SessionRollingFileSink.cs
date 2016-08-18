using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Sessions;
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
        private readonly Dictionary<string, RollingFileSink> _sinks = new Dictionary<string, RollingFileSink>();

        public SessionRollingFileSink(ISessionManager sessionManager,
                                      string logPath)
        {
            Contract.Requires(sessionManager != null);
            Contract.Requires(string.IsNullOrWhiteSpace(logPath) == false);

            _sessionManager = sessionManager;
            _sessionManager.SessionRemoved += SessionRemoved;
            _logPath = logPath;
        }
        
        public void Emit(LogEvent logEvent)
        {
            var sink = GetSink();
            
            sink.Emit(logEvent);
        }

        private RollingFileSink GetSink()
        {
            Contract.Ensures(Contract.Result<RollingFileSink>() != null);

            var session = _sessionManager.Current;

            var sessionId = session == null ? string.Empty : session.Id;
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

        private void SessionRemoved(object sender, SessionEventArgs e)
        {
            Contract.Requires(e?.Session?.Id != null);
            
            var session = e.Session;

            RollingFileSink sink;

            if (_sinks.TryGetValue(session.Id, out sink))
            {
                Contract.Assume(sink != null);

                sink.Dispose();
            }

            _sinks.Remove(session.Id);
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
