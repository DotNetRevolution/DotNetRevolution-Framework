using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.Extensions
{
    public static class HtmlHelperExtension
    {
        public static bool PartialViewExists(this HtmlHelper helper, string viewName)
        {
            Contract.Requires(helper != null);
            Contract.Requires(string.IsNullOrWhiteSpace(viewName) == false);

            var engines = ViewEngines.Engines;
            Contract.Assume(engines != null);

            var view = engines.FindPartialView(helper.ViewContext, viewName);
            Contract.Assume(view != null);
            
            return view.View != null;
        }
    }
}
