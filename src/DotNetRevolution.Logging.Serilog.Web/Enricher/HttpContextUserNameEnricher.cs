using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;
using System.Web;

namespace DotNetRevolution.Logging.Serilog.Web.Enricher
{
    public class HttpContextUserNameEnricher : ILogEventEnricher
    {
        public const string PropertyName = "UserName";
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);
            Contract.Assume(HttpContext.Current?.User != null);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyName, HttpContext.Current.User.Identity.Name));
        }
    }
}
