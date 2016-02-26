using System.Web.Mvc;

namespace LHSCampaign.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return User.Identity.IsAuthenticated ?
                RedirectToAction("Home", controllerName: "Welcome") :
                RedirectToAction("GetClass", controllerName: "Candidates");
        }

        [Route("GPlus")]
        public ActionResult GPlus()
        {
            return View();
        }
    }
}