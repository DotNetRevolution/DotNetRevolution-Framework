using DotNetRevolution.Core.Session;
using System.Web;

namespace DotNetRevolution.MVC.Session
{
    public class HttpContextSessionManager : SessionManager
    {
        public override ISession GetCurrentSession()
        {
            if (HttpContext.Current?.Session == null)
            {
                return null;
            }

            var session = HttpContext.Current.Session;
                        
            return this[session.SessionID];
        }        
    }
}
