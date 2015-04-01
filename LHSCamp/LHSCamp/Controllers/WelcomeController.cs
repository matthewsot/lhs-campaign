using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHSCamp.Models;
using Microsoft.AspNet.Identity;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        private LCDB db = new LCDB();

        public ActionResult Candidate()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null || !user.IsCandidate)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Id = user.Candidate.Id;

            ViewBag.Confirmed = user.IsConfirmed;
            ViewBag.Email = user.Email;
            ViewBag.Position = user.Candidate.Position;
            ViewBag.Reasons = user.Candidate.Reasons ?? string.Empty;
            if (TempData.ContainsKey("Uploaded"))
            {
                ViewBag.Uploaded = (bool)TempData["Uploaded"];
            }
            else
            {
                ViewBag.Uploaded = false;
            }

            return View();
        }

        // Thanks! http://stackoverflow.com/questions/5193842/file-upload-asp-net-mvc-3-0
        private bool UploadPicture(HttpPostedFileBase file, User user, string folderPath, bool isProfile)
        {
            var allowedExts = new[] { ".jpg", ".png", ".gif" };

            // Verify that the user selected a file
            if (file == null || file.FileName == null || file.ContentLength <= 0)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExts.Contains(extension))
            {
                return false;
            }

            var imagesFolder = Server.MapPath(folderPath);
            var path = imagesFolder + user.Id + extension;

            Directory.CreateDirectory(imagesFolder);

            // Remove the existing picture and log an image change
            if (isProfile && user.Candidate.ProfilePic != null)
            {
                var oldPic = Server.MapPath("~" + user.Candidate.ProfilePic);
                System.IO.File.Delete(oldPic);

                var entry = user.Candidate.ProfilePic;
                var currLog = db.Log.FirstOrDefault(log => log.Type == "Image Changed/Removed" && log.Entry == entry);
                if (currLog == null)
                {
                    db.Log.Add(new LogEntry() // So I can refresh the image in Cloudflare, if necessary
                    {
                        Type = "Image Changed/Removed",
                        Entry = entry
                    });
                }
            }
            else if (!isProfile && user.Candidate.CoverPhoto != null)
            {
                var oldPic = Server.MapPath("~" + user.Candidate.CoverPhoto);
                System.IO.File.Delete(oldPic);

                var entry = user.Candidate.CoverPhoto;
                var currLog = db.Log.FirstOrDefault(log => log.Type == "Image Changed/Removed" && log.Entry == entry);
                if (currLog == null)
                {
                    db.Log.Add(new LogEntry() // So I can refresh the image in Cloudflare, if necessary
                    {
                        Type = "Image Changed/Removed",
                        Entry = entry
                    });
                }
            }

            file.SaveAs(path); // Upload the new pic
            if (isProfile)
            {
                user.Candidate.ProfilePic = "/Content/Images/Candidates/" + user.Id + extension;
            }
            else
            {
                user.Candidate.CoverPhoto = "/Content/Images/Covers/" + user.Id + extension;
            }
            db.SaveChanges();
            return true;
        }

        [HttpPost]
        public ActionResult UploadProfile(HttpPostedFileBase file)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!user.IsCandidate)
            { // Only candidates can upload profile pictures
                return RedirectToAction("Index", controllerName: "Home");
            }

            this.UploadPicture(file, user, "~/Content/Images/Candidates/", true);

            TempData["Uploaded"] = true; // Not the best way to do this, but it'll do for now
            return RedirectToAction("Candidate", controllerName: "Welcome");
        }

        [HttpPost]
        public ActionResult UploadCover(HttpPostedFileBase file)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (!user.IsCandidate)
            {
                return RedirectToAction("Index", controllerName: "Home");
            }

            this.UploadPicture(file, user, "~/Content/Images/Covers/", false);

            TempData["Uploaded"] = true;
            return RedirectToAction("Candidate", controllerName: "Welcome");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}