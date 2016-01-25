using System.Diagnostics.Contracts;
using System.Web.Mvc;
using DotNetRevolution.Core.Command;
using DotNetRevolution.Core.Query;

namespace DotNetRevolution.MVC
{
    public abstract class DotNetRevolutionController : Controller
    {
        public ICommandDispatcher CommandDispatcher { get; private set; }
        public IQueryDispatcher QueryDispatcher { get; private set; }

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