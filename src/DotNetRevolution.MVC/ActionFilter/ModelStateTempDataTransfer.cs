using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace DotNetRevolution.MVC.ActionFilter
{
    /// <summary>
    /// A base class for Action Filters that need to transfer ModelState to/from TempData
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ModelStateTempDataTransferAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransferAttribute).FullName;

        /// <summary>
        /// Exports the current ModelState to TempData (available on the next request).
        /// </summary>       
        protected static void ExportModelStateToTempData(ControllerContext context)
        {
            Contract.Requires(context != null);

            Contract.Assume(context.Controller?.TempData != null);
            Contract.Assume(context.Controller.ViewData != null);

            context.Controller.TempData[Key] = context.Controller.ViewData.ModelState;
        }

        /// <summary>
        /// Populates the current ModelState with the values in TempData
        /// </summary>
        protected static void ImportModelStateFromTempData(ControllerContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Controller != null);
            Contract.Requires(context.Controller.TempData != null);
            Contract.Requires(context.Controller.ViewData?.ModelState != null);

            var prevModelState = context.Controller.TempData[Key] as ModelStateDictionary;
            context.Controller.ViewData.ModelState.Merge(prevModelState);
        }

        /// <summary>
        /// Removes ModelState from TempData
        /// </summary>
        protected static void RemoveModelStateFromTempData(ControllerContext context)
        {
            Contract.Requires(context?.Controller?.TempData != null);

            context.Controller.TempData[Key] = null;
        }
    }
}
