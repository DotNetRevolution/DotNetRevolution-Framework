using System.Diagnostics.Contracts;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotNetRevolution.MVC.ViewEngine
{
    public class ActionPartialRazorViewEngine : RazorViewEngine
    {
        private const string ActionToken = "%ACTION%";

        public ActionPartialRazorViewEngine()
        {
            RegisterPartialLocationFormats();
            RegisterViewLocationFormats();
        }

        private void RegisterPartialLocationFormats()
        {
            var areaPartialLocationFormats = new[] 
                {
                    "~/Areas/{2}/Views/{1}/" + ActionToken + "/{0}.cshtml",
                    "~/Areas/{2}/Views/{1}/Partials/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/Partials/{0}.cshtml",
                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
                };

            var partialLocationFormats = new[]
                {
                    "~/Views/{1}/" + ActionToken + "/{0}.cshtml",
                    "~/Views/{1}/Partials/{0}.cshtml",
                    "~/Views/Shared/Partials/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                };

            PartialViewLocationFormats = partialLocationFormats;
            AreaPartialViewLocationFormats = areaPartialLocationFormats;
        }

        private void RegisterViewLocationFormats()
        {
            var areaViewLocationFormats = new[] 
                {
                    "~/Areas/{2}/Views/{1}/" + ActionToken + "/{0}.cshtml",
                    "~/Areas/{2}/Views/{1}/{0}.cshtml",
                };

            var viewLocationFormats = new[] 
                {
                    "~/Views/{1}/" + ActionToken + "/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                };

            ViewLocationFormats = viewLocationFormats;
            AreaViewLocationFormats = areaViewLocationFormats;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            Contract.Assume(controllerContext?.RouteData?.Values != null);

            return base.CreatePartialView(controllerContext, GetPath(controllerContext.RouteData, partialPath));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            Contract.Assume(controllerContext?.RouteData?.Values != null);

            return base.CreateView(controllerContext, GetPath(controllerContext.RouteData, viewPath), masterPath);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            Contract.Assume(controllerContext?.RouteData?.Values != null);

            return base.FileExists(controllerContext, GetPath(controllerContext.RouteData, virtualPath));
        }

        private static string GetPath(RouteData routeData, string path)
        {
            Contract.Requires(routeData?.Values != null);

            object action;

            if (routeData.Values.TryGetValue("action", out action))
            {
                Contract.Assume(!string.IsNullOrWhiteSpace(path));

                return path.Replace(ActionToken, action as string);
            }

            return path;
        }
    }
}