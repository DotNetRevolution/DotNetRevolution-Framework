using DotNetRevolution.Core.Authentication;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Enricher
{
    public class IdentityEnricher : ILogEventEnricher
    {
        public const string PropertyName = "UserName";

        private readonly IIdentityManager _identityManager;

        public IdentityEnricher(IIdentityManager identityManager)
        {
            Contract.Requires(identityManager != null);

            _identityManager = identityManager;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);

            var identity = _identityManager.Current;
            
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyName, identity == null ? string.Empty : identity.Name));
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_identityManager != null);
        }
    }
}
