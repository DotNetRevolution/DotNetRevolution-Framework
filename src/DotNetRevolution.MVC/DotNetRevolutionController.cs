using System.Diagnostics.Contracts;
using System.Web.Mvc;
using DotNetRevolution.Core.Commanding;
using DotNetRevolution.Core.Querying;

namespace DotNetRevolution.MVC
{
    public abstract class DotNetRevolutionController : Controller
    {
        public ICommandDispatcher CommandDispatcher { get; }
        public IQueryDispatcher QueryDispatcher { get; }

        protected DotNetRevolutionController(ICommandDispatcher commandDispatcher,
                                     IQueryDispatcher queryDispatcher)
        {
            Contract.Requires(commandDispatcher != null, "commandDispatcher");
            Contract.Requires(queryDispatcher != null, "queryDispatcher");

            CommandDispatcher = commandDispatcher;
            QueryDispatcher = queryDispatcher;
        }
    }
}