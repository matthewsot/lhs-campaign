using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Candidate", controllerName: "Welcome");
            }
            return RedirectToAction("Index", controllerName: "Home");
        }
        [Route("GPlus")]
        public ActionResult GPlus()
        {
            return View();
        }
    }
}