using DotNetRevolution.Core.Sessions;
using System.Web;

namespace DotNetRevolution.Web.Sessions
{
    public class HttpContextSessionManager : SessionManager
    {
        public override ICurrentSession Current
        {
            get
            {
                if (HttpContext.Current?.Session == null)
                {
                    return null;
                }

                var session = HttpContext.Current.Session;

                return new HttpCurrentSession(HttpContext.Current.Session);
            }
        }
    }
}
