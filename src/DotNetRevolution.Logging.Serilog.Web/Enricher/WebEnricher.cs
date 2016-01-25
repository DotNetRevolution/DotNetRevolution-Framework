using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.Contracts;
using System.Web;

namespace DotNetRevolution.Logging.Serilog.Web.Enricher
{
    public class WebEnricher : ILogEventEnricher
    {
        public const string BrowserNamePropertyName = "BrowserName";
        public const string BrowserVersionPropertyName = "BrowserVersion";
        public const string UserAgentPropertyName = "UserAgent";
        public const string UrlPropertyName = "Url";
        public const string SessionIdPropertyName = "SessionId";
        public const string HttpMethodPropertyName = "HttpMethod";
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);

            var httpContext = HttpContext.Current;
            Contract.Assume(httpContext != null);
            Contract.Assume(httpContext.Session != null);
            Contract.Assume(httpContext.Request.Browser != null);

            var session = httpContext.Session;
            var browser = httpContext.Request.Browser;

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(BrowserNamePropertyName, browser.Browser));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(BrowserVersionPropertyName, browser.Version));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(SessionIdPropertyName, session.SessionID));
        }
    }
}
