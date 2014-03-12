using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Owin;
using LHSCamp.Models;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Candidate()
        {
            var userId = User.Identity.GetUserId();
            using (var db = new LCDB())
            {
                var currUser = db.Users.FirstOrDefault(u => u.Id == userId);
                if (currUser == null || !currUser.IsCandidate)
                    return RedirectToAction("Index", "Home");

                ViewBag.Confirmed = currUser.IsConfirmed;
                ViewBag.Email = currUser.Email;
                ViewBag.Position = currUser.Candidate.Position;
                ViewBag.Reasons = currUser.Candidate.Reasons ?? "";
            }
            return View();
        }
        //Thanks! http://stackoverflow.com/questions/5193842/file-upload-asp-net-mvc-3-0
        [HttpPost]
        public ActionResult UploadProfile(HttpPostedFileBase file)
        {
            using (var db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var currUser = db.Users.FirstOrDefault(u => u.Id == userId);

                if (!currUser.IsCandidate)
                    return RedirectToAction("Index", controllerName: "Home");

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var allowedExts = new string[] { ".jpg", ".png", ".gif" };
                    if (allowedExts.Contains(extension))
                    {
                        var imagesFolder = Server.MapPath("~/Content/Images/Candidates/");
                        var path = imagesFolder + userId + extension;

                        if (!Directory.Exists(imagesFolder))
                            Directory.CreateDirectory(imagesFolder);

                        //Remove the existing picture
                        if (currUser.Candidate.ProfilePic != null)
                        {
                            var oldPic = Server.MapPath("~" + currUser.Candidate.ProfilePic);
                            if (System.IO.File.Exists(oldPic))
                                System.IO.File.Delete(oldPic);
                        }

                        file.SaveAs(path);
                        currUser.Candidate.ProfilePic = "/Content/Images/Candidates/" + userId + extension;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", controllerName: "Home");
            }
        }
    }
}