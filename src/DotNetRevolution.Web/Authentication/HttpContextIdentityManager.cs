using System.Security.Principal;
using DotNetRevolution.Core.Authentication;
using System.Web;
using System.Diagnostics.Contracts;

namespace DotNetRevolution.Web.Authentication
{
    public class HttpContextIdentityManager : IIdentityManager
    {
        public IIdentity Current
        {
            get
            {
                Contract.Assume(HttpContext.Current?.User != null);
                
                return HttpContext.Current.User.Identity;
            }
        }
    }
}
