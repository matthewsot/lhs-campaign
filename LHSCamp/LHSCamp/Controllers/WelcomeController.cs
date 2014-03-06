using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Candidate()
        {
            return View();
        }
    }
}