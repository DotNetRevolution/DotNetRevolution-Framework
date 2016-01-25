using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.ActionFilter
{
    /// <summary>
    /// An Action Filter for importing ModelState from TempData.
    /// You need to decorate your GET actions with this when using the <see cref="ValidateModelStateAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Useful when following the PRG (Post, Redirect, Get) pattern.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ImportModelStateFromTempDataAttribute : ModelStateTempDataTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Contract.Requires(filterContext?.Controller?.TempData != null);
            Contract.Requires(filterContext.Controller.ViewData?.ModelState != null);

            // Only copy from TempData if we are rendering a View/Partial
            if (filterContext.Result is ViewResult)
            {
                ImportModelStateFromTempData(filterContext);
            }
            else
            {
                // remove it
                RemoveModelStateFromTempData(filterContext);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
