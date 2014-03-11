﻿using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class AccountAPIController : ApiController
    {
        private LCDB db = new LCDB();

        [HttpPost]
        [Route("API/Account/CheckName")]
        [AllowAnonymous]
        public IHttpActionResult CheckName(UserNameModel model)
        {
            bool exists = (db.Users.FirstOrDefault(u => u.UserName == model.Username) != null);
            if (exists)
                return Ok("exists");
            else
                return Ok("new");
        }

        [HttpPost]
        [Route("API/Account/SetEmail")]
        public IHttpActionResult SetEmail(SetEmailModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null)
                return Ok("no user");

            user.Email = model.email;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetPosition")]
        public IHttpActionResult SetPosition(SetPositionModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null)
                return Ok("no user");

            if (user.IsCandidate)
            {
                user.Candidate.Position = model.position;
                db.SaveChanges();
            }
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetReasons")]
        public IHttpActionResult SetReasons(SetReasonsModel model)
        {
            var userId = User.Identity.GetUserId();
            
            var user = db.Users.Find(userId);
            if (user == null)
                return Ok("no user");

            var candidate = user.Candidate;
            if (candidate == null)
                return Ok("not candidate");

            candidate.Reasons = model.reasons;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetSocial")]
        public IHttpActionResult SetSocial(SetSocialModel model)
        {
            if (model.facebook != null)
                model.facebook = model.facebook.Trim();

            if (model.facebook == "")
                model.facebook = null;

            var userId = User.Identity.GetUserId();

            var user = db.Users.Find(userId);
            if (user == null)
                return Ok("no user");

            var candidate = user.Candidate;
            if (candidate == null)
                return Ok("no candidate");

            candidate.Facebook = model.facebook;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetPass")]
        public IHttpActionResult SetPass(SetPassModel model)
        {
            using (var UserManager = new Microsoft.AspNet.Identity.UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.currPass, model.newPass);
                if (result.Succeeded)
                    return Ok("set");
                else
                    return Ok("nope");
            }
        }

        [HttpPost]
        [Route("API/Account/Register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            using (var UserManager = new Microsoft.AspNet.Identity.UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                var Errors = new List<string>();

                //TODO: Should be validating with ModelState
                if (model.Password.Length <= 6) Errors.Add("Password");
                if (model.Year != 2016 && model.Year != 2017) Errors.Add("Year");
                if (model.Position != null && model.Position.Length > 50) Errors.Add("Position");
                if (model.FullName != null && model.FullName.Length > 50) Errors.Add("FullName");
                if (db.Users.Count(usr => usr.UserName == model.Username) > 0) Errors.Add("Username");

                if (Errors.Count > 0)
                    return Ok(string.Join(",", Errors) + ",");

                var user = new User() { UserName = model.Username, Email = model.Email };
                if (!string.IsNullOrWhiteSpace(model.Position))
                {
                    if (string.IsNullOrWhiteSpace(model.FullName))
                        model.FullName = model.Username;

                    //create candidate for user
                    user.Candidate = new Candidate()
                    {
                        Owner = user,
                        Position = model.Position,
                        Name = model.FullName
                    };
                }

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GetConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmUser", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");
                    return Ok("GOOD");
                }
                else
                {
                    //Errors
                    return Ok("WOOPS");
                }
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
