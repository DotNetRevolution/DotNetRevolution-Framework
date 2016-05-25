using System.Web.Mvc;

namespace DotNetRevolution.EventSourcing.Web.Areas.Audit.Controllers
{
    public class HomeController : Controller
    {
        // GET: Audit/Audit
        public ActionResult Index()
        {
            return View();
        }
    }
}