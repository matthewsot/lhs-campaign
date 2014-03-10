using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class CreatePhotoController : Controller
    {
        public ActionResult Cover()
        {
            ViewBag.Creating = "Cover Photo";
            ViewBag.Width = 1106.3; //851
            ViewBag.Height = 409.5; //315
            ViewBag.Grid = 25;
            ViewBag.PerLine = 4;
            return View("Creator");
        }

        public ActionResult Profile()
        {
            ViewBag.Creating = "Profile Picture";
            ViewBag.Width = 500;
            ViewBag.Height = 500;
            ViewBag.Grid = 25;
            ViewBag.PerLine = 2;
            return View("Creator");
        }
    }
}