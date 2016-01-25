using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Web.Mvc;
using DotNetRevolution.MVC.Extensions;
using DotNetRevolution.Core.Serialization;

namespace DotNetRevolution.MVC.ActionFilter
{
    /// <summary>
    /// An ActionFilter for automatically validating ModelState before a controller action is executed.
    /// Performs a Redirect if ModelState is invalid. Assumes the <see cref="ImportModelStateFromTempDataAttribute"/> is used on the GET action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateModelStateAttribute : ModelStateTempDataTransferAttribute
    {
        private readonly ISerializer _serializer;

        public ValidateModelStateAttribute(ISerializer serializer)
        {
            Contract.Requires(serializer != null);

            _serializer = serializer;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Contract.Requires(filterContext != null);
            
            Contract.Assume(filterContext.Controller?.ViewData?.ModelState != null);
            Contract.Assume(filterContext.Controller.ViewData.ModelState.IsValid || filterContext.HttpContext != null);

            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ProcessAjax(filterContext);
                }
                else
                {
                    ProcessNormal(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }
        
        private void ProcessAjax(ActionExecutingContext filterContext)
        {
            Contract.Requires(filterContext != null);
                        
            Contract.Assume(filterContext.Controller?.ViewData?.ModelState != null);

            var errors = filterContext.Controller.ViewData.ModelState.ToSerializableDictionary();
            var json = _serializer.Serialize(errors);

            // send 400 status code (Bad Request)
            filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, json);
        }

        private static void ProcessNormal(ActionExecutingContext filterContext)
        {
            Contract.Requires(filterContext != null);

            // Export ModelState to TempData so it's available on next request
            ExportModelStateToTempData(filterContext);

            Contract.Assume(filterContext.RouteData?.Values != null);

            // redirect back to GET action
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_serializer != null);
        }
    }
}
