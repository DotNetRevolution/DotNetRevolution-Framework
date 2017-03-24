using System.Web.Mvc;

namespace DotNetRevolution.EventSourcing.Auditor.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
