using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHSCampaign.Models;
using Microsoft.AspNet.Identity;

namespace LHSCampaign.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private LCDb db = new LCDb();

        public ActionResult Candidate()
        {
            var candidate = db.Users.Find(User.Identity.GetUserId());
            if (candidate == null)
            {
                return RedirectToAction("Class", "Candidates");
            }
            
            var model = new CandidateViewModel(candidate);
            var existingFacebook = candidate.ExternalLinks.FirstOrDefault(link => link.Label == "FB EVENT")?.Link;
            ViewBag.SocialLink = string.IsNullOrWhiteSpace(existingFacebook) ? "" : existingFacebook;

            if (TempData.ContainsKey("Uploaded"))
            {
                ViewBag.Uploaded = (bool)TempData["Uploaded"];
            }
            else
            {
                ViewBag.Uploaded = false;
            }

            return View(model);
        }

        // Thanks! http://stackoverflow.com/questions/5193842/file-upload-asp-net-mvc-3-0
        private bool UploadPicture(HttpPostedFileBase file, Candidate candidate, string folderPath, bool isProfile)
        {
            var allowedExts = new[] { ".jpg", ".png", ".gif", ".jpeg" };

            // Verify that the Candidate selected a file
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
            var path = imagesFolder + candidate.Id + extension;

            Directory.CreateDirectory(imagesFolder);

            // Remove the existing picture and log an image change
            if (isProfile && candidate.ProfilePicture != null)
            {
                var oldPic = Server.MapPath("~" + candidate.ProfilePicture);
                System.IO.File.Delete(oldPic);

                var entry = candidate.ProfilePicture;
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
            else if (!isProfile && candidate.CoverPhoto != null)
            {
                var oldPic = Server.MapPath("~" + candidate.CoverPhoto);
                System.IO.File.Delete(oldPic);

                var entry = candidate.CoverPhoto;
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
                candidate.ProfilePicture = "/Content/Images/Candidates/" + candidate.Id + extension;
            }
            else
            {
                candidate.CoverPhoto = "/Content/Images/Covers/" + candidate.Id + extension;
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

            UploadPicture(file, user, "~/Content/Images/Candidates/", true);

            TempData["Uploaded"] = true; // Not the best way to do this, but it'll do for now
            return RedirectToAction("Candidate", controllerName: "Manage");
        }

        [HttpPost]
        public ActionResult UploadCover(HttpPostedFileBase file)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            UploadPicture(file, user, "~/Content/Images/Covers/", false);

            TempData["Uploaded"] = true;
            return RedirectToAction("Candidate", controllerName: "Manage");
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