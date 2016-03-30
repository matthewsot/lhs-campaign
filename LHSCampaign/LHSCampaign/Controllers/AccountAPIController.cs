﻿using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using LHSCampaign.Models;

namespace LHSCampaign.Controllers
{
    [Authorize]
    public class AccountAPIController : ApiController
    {
        private LCDb db = new LCDb();

        [HttpPost]
        [Route("API/Account/CheckName")]
        [AllowAnonymous]
        public IHttpActionResult CheckName(UserNameModel model)
        {
            var exists = (db.Users.FirstOrDefault(u => u.UserName == model.username) != null);
            return Ok(exists ? "exists" : "new");
        }

        [HttpPost]
        [Route("API/Account/SetEmail")]
        public IHttpActionResult SetEmail(SetEmailModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return Ok("no user");
            }

            user.Email = model.email;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetPosition")]
        public IHttpActionResult SetPosition(SetPositionModel model)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (user == null)
            {
                return Ok("no user");
            }

            user.Position = model.position;
            db.SaveChanges();

            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetReasons")]
        public IHttpActionResult SetReasons(SetReasonsModel model)
        {
            var candidate = db.Users.Find(User.Identity.GetUserId());

            if (candidate == null)
            {
                return Ok("no user");
            }

            candidate.Platform = model.reasons;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetSocial")]
        public IHttpActionResult SetSocial(SetSocialModel model)
        {
            model.facebook = string.IsNullOrWhiteSpace(model.facebook) ? null : model.facebook.Trim();

            var candidate = db.Users.Find(User.Identity.GetUserId());
            if (candidate == null)
            {
                return Ok("no user");
            }

            var existingFacebook = candidate.ExternalLinks.FirstOrDefault(link => link.Label == "FB EVENT");
            if (existingFacebook != null)
            {
                candidate.ExternalLinks.Remove(existingFacebook);
            }

            if (model.facebook != null)
            {
                candidate.ExternalLinks.Add(new ExternalLink()
                {
                    Label = "FB EVENT",
                    Link = model.facebook
                });
            }
            db.SaveChanges();

            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/Register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            using (var userManager = new UserManager<Candidate>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<Candidate>(db)))
            {
                var errors = new List<string>();

                // TODO: Should be validating with ModelState
                if (model.Password.Length <= 6) errors.Add("Password");
                if (!(model.Year <= 2019 && model.Year >= 2017)) errors.Add("Year");
                if (string.IsNullOrWhiteSpace(model.Position) || model.Position.Length > 50) errors.Add("Position");
                model.Position = model.Position.ToLower();
                if (string.IsNullOrWhiteSpace(model.FullName) || model.FullName.Length > 50) errors.Add("FullName");
                if (db.Users.Count(usr => usr.UserName == model.Username) > 0) errors.Add("Username");

                if (errors.Count > 0)
                {
                    return Ok(string.Join(",", errors) + ",");
                }

                var candidate = new Candidate
                {
                    UserName = model.Username,
                    Email = model.Email,
                    GraduationYear = model.Year,
                    Position = model.Position,
                    Name = model.FullName,
                    IsConfirmed = true
                };
                
                var preConf = db.PreConfs.FirstOrDefault(conf => conf.Email == model.Email.ToLower());
                if (preConf != null)
                {
                    candidate.IsConfirmed = true;
                    db.PreConfs.Remove(preConf);
                }

                var result = await userManager.CreateAsync(candidate, model.Password);
                db.SaveChanges();
                return Ok(result.Succeeded ? "GOOD" : string.Join(",", errors));
            }
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
