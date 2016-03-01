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
        public const string HttpMethodPropertyName = "HttpMethod";
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Contract.Assume(propertyFactory != null);
            Contract.Assume(logEvent != null);

            var httpContext = HttpContext.Current;
            Contract.Assume(httpContext != null);

            var request = httpContext.Request; 
                       
            var browser = request.Browser;
            Contract.Assume(browser != null);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(BrowserNamePropertyName, browser.Browser));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(BrowserVersionPropertyName, browser.Version));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(HttpMethodPropertyName, request.HttpMethod));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(UrlPropertyName, request.RawUrl));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(UserAgentPropertyName, request.UserAgent));
        }
    }
}
