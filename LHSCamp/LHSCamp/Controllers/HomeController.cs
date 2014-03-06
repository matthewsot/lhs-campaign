using LHSCamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using(LCDB db = new LCDB())
            {
                var currUser = db.Users.FirstOrDefault(user => user.Id == User.Identity.GetUserId());
                if(currUser != null && currUser.IsCandidate == true && string.IsNullOrWhiteSpace(currUser.Candidate.ProfilePic))
                {
                    return RedirectToAction("Candidate", controllerName: "Welcome");
                }
            }
            return View();
        }
    }
}