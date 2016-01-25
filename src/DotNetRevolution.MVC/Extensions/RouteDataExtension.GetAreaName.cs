using System.Diagnostics.Contracts;
using System.Web.Routing;

namespace DotNetRevolution.MVC.Extensions
{
    public static class RouteDataExtension
    {
        public static string GetAreaName(this RouteData routeData)
        {
            Contract.Requires(routeData?.DataTokens != null);

            object area;

            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }

            return null;
        } 
    }
}