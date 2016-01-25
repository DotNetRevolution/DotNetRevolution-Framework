using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.ViewEngine
{
    public class PartialsSubfolderViewEngine : RazorViewEngine
    {
        public PartialsSubfolderViewEngine()
        {
            Contract.Assume(PartialViewLocationFormats != null);
            Contract.Assume(AreaPartialViewLocationFormats != null);

            var newLocationFormat = new[]
                {
                    "~/Views/{1}/Partials/{0}.cshtml",
                };

            var newAreaLocationFormat = new[]
                {
                    "~/Areas/{2}/Views/{1}/Partials/{0}.cshtml",
                };

            PartialViewLocationFormats = PartialViewLocationFormats.Union(newLocationFormat).ToArray();
            AreaPartialViewLocationFormats = AreaPartialViewLocationFormats.Union(newAreaLocationFormat).ToArray();
        }
    }
}