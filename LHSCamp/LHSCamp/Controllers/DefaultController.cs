using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return User.Identity.IsAuthenticated ?
                RedirectToAction("Candidate", controllerName: "Welcome") :
                RedirectToAction("GetClass", controllerName: "Candidates");
        }

        [Route("GPlus")]
        public ActionResult GPlus()
        {
            return View();
        }
    }
}