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
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Candidate()
        {
            return View();
        }
        //Thanks! http://stackoverflow.com/questions/5193842/file-upload-asp-net-mvc-3-0
        [HttpPost]
        [Authorize]
        public ActionResult UploadProfile(HttpPostedFileBase file)
        {
            using (var db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var currUser = db.Users.FirstOrDefault(u => u.Id == userId);
                if (!User.Identity.IsAuthenticated || !(currUser.IsCandidate/* && currUser.IsConfirmed*/))
                    return RedirectToAction("Index", controllerName: "Home");

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    if (".jpg,.png,.gif,".Contains(extension + ","))
                    {
                        var imagesFolder = Server.MapPath("~/Content/Images/Candidates/");
                        var path = imagesFolder + userId + extension;

                        if (!Directory.Exists(imagesFolder))
                            Directory.CreateDirectory(imagesFolder);

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