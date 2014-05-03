using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using(var db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var currUser = db.Users.Find(userId);
                if(currUser != null && currUser.IsCandidate && string.IsNullOrWhiteSpace(currUser.Candidate.ProfilePic))
                {
                    return RedirectToAction("Candidate", controllerName: "Welcome");
                }
            }
            return View();
        }
    }
}