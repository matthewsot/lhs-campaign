using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class CreatePhotoController : Controller
    {
        public ActionResult Cover()
        {
            return View();
        }

        [Route("CreatePhoto/Profile")]
        public ActionResult CreateProfile()
        {
            return View("Profile");
        }
    }
}