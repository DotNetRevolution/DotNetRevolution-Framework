using DotNetRevolution.Core.Sessions;
using System.Web;

namespace DotNetRevolution.MVC.Session
{
    public class HttpContextSessionManager : SessionManager
    {
        public override ISession Current
        {
            get
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
}
