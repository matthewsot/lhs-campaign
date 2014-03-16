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
        private LCDB db = new LCDB();
        // GET: Welcome
        public ActionResult Candidate()
        {
            var userId = User.Identity.GetUserId();
            var currUser = db.Users.FirstOrDefault(u => u.Id == userId);
            if (currUser == null || !currUser.IsCandidate)
                return RedirectToAction("Index", "Home");

            ViewBag.Confirmed = currUser.IsConfirmed;
            ViewBag.Email = currUser.Email;
            ViewBag.Position = currUser.Candidate.Position;
            ViewBag.Reasons = currUser.Candidate.Reasons ?? "";
            if (TempData.ContainsKey("Uploaded"))
            {
                ViewBag.Uploaded = ((bool)TempData["Uploaded"]);
            }
            else
            {
                ViewBag.Uploaded = false;
            }
            return View();
        }
        string[] allowedExts = new string[] { ".jpg", ".png", ".gif" };
        //Thanks! http://stackoverflow.com/questions/5193842/file-upload-asp-net-mvc-3-0
        [HttpPost]
        public ActionResult UploadProfile(HttpPostedFileBase file)
        {
            var userId = User.Identity.GetUserId();
            var currUser = db.Users.FirstOrDefault(u => u.Id == userId);

            if (!currUser.IsCandidate)
                return RedirectToAction("Index", controllerName: "Home");

            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
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

                        var entry = currUser.Candidate.ProfilePic.ToString();
                        var currLog = db.Log.FirstOrDefault(log => log.Type == "Image Changed/Removed" && log.Entry == entry);
                        if (currLog == null)
                        {
                            db.Log.Add(new LogEntry() //So I can refresh the image in Cloudflare, if necessary
                            {
                                Type = "Image Changed/Removed",
                                Entry = entry
                            });
                        }
                    }

                    file.SaveAs(path);
                    currUser.Candidate.ProfilePic = "/Content/Images/Candidates/" + userId + extension;
                    db.SaveChanges();
                }
            }
            TempData["Uploaded"] = true; //Not the best way to do this, but it'll do for now
            return RedirectToAction("Candidate", controllerName: "Welcome");
        }

        [HttpPost]
        public ActionResult UploadCover(HttpPostedFileBase file)
        {
            var userId = User.Identity.GetUserId();
            var currUser = db.Users.FirstOrDefault(u => u.Id == userId);

            if (!currUser.IsCandidate)
                return RedirectToAction("Index", controllerName: "Home");

            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                if (allowedExts.Contains(extension))
                {
                    var imagesFolder = Server.MapPath("~/Content/Images/Covers/");
                    var path = imagesFolder + userId + extension;

                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    //Remove the existing picture
                    if (currUser.Candidate.CoverPhoto != null)
                    {
                        var oldPic = Server.MapPath("~" + currUser.Candidate.CoverPhoto);
                        if (System.IO.File.Exists(oldPic))
                            System.IO.File.Delete(oldPic);

                        var entry = currUser.Candidate.ProfilePic.ToString();
                        var currLog = db.Log.FirstOrDefault(log => log.Type == "Image Changed/Removed" && log.Entry == entry);
                        if (currLog == null)
                        {
                            db.Log.Add(new LogEntry() //So I can refresh the image in Cloudflare, if necessary
                            {
                                Type = "Image Changed/Removed",
                                Entry = entry
                            });
                        }
                    }

                    file.SaveAs(path);
                    currUser.Candidate.CoverPhoto = "/Content/Images/Covers/" + userId + extension;
                    db.SaveChanges();
                }
            }
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