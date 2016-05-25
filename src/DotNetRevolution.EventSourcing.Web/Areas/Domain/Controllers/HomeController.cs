using System.Web.Mvc;

namespace DotNetRevolution.EventSourcing.Web.Areas.Domain.Controllers
{
    public class HomeController : Controller
    {
        // GET: Domain/Main
        public ActionResult Index()
        {
            return View();
        }
    }
}