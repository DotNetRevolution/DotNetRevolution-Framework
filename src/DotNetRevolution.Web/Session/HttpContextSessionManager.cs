using DotNetRevolution.Core.Session;
using System.Diagnostics.Contracts;
using System.Web;

namespace DotNetRevolution.MVC.Session
{
    public class HttpContextSessionManager : SessionManager
    {
        public override ISession GetCurrentSession()
        {
            Contract.Assume(HttpContext.Current?.Session != null);

            var session = HttpContext.Current.Session;
                        
            return this[session.SessionID];
        }        
    }
}
