using System.Web;
using System.Web.Mvc;

namespace DotNetRevolution.EventSourcing.Auditor.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
