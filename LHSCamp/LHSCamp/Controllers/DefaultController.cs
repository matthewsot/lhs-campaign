using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", controllerName: "Home");
            }
            return View();
        }
        [Route("GPlus")]
        public ActionResult GPlus()
        {
            return View();
        }
    }
}