using System.Web;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Logging.Serilog.Enricher
{
    public class HttpContextUserNameEnricher : ILogEventEnricher
    {
        public const string PropertyName = "UserName";
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);

            var httpContext = HttpContext.Current;
            Contract.Assume(httpContext != null);

            var user = httpContext.User;
            Contract.Assume(user != null);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyName, user.Identity.Name));
        }
    }
}
