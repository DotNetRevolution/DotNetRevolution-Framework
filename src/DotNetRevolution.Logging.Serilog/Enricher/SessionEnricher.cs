using DotNetRevolution.Core.Sessions;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Enricher
{
    public class SessionEnricher : ILogEventEnricher
    {
        public const string PropertyName = "SessionId";

        private readonly ISessionManager _sessionManager;

        public SessionEnricher(ISessionManager sessionManager)
        {
            Contract.Requires(sessionManager != null);

            _sessionManager = sessionManager;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);

            var session = _sessionManager.Current;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyName, session == null ? string.Empty : session.Id));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_sessionManager != null);
        }
    }
}
