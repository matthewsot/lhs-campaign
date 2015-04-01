using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
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
            var exists = (db.Users.FirstOrDefault(u => u.UserName == model.username) != null);
            return Ok(exists ? "exists" : "new");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("API/Account/StartResetPass")]
        public IHttpActionResult StartResetPassword(UserNameModel model)
        {
            using (var userManager = new UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                //Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
                if (Startup.DataProtectionProvider != null)
                {
                    userManager.PasswordResetTokens = new DataProtectorTokenProvider(Startup.DataProtectionProvider.Create("PasswordReset"));
                    userManager.UserConfirmationTokens = new DataProtectorTokenProvider(Startup.DataProtectionProvider.Create("ConfirmUser"));
                }
                var user = db.Users.FirstOrDefault(u => u.UserName == model.username);

                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                {
                    return Ok("problem");
                }

                //Thanks! http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
                var settings = Config.GetValues(new[] { "SMTP Server", "SMTP Port", "SMTP User", "SMTP Pass" });
                var mail = new MailMessage();
                var smtpServer = new SmtpClient(settings["SMTP Server"]);
                mail.From = new MailAddress("postmaster@lhscampaign.cf", "LHS|Campaign");
                var userName = User.Identity.GetUserName();
                mail.To.Add(new MailAddress(user.Email, userName));
                mail.Subject = "Reset Your Password";
                mail.Body = "Please visit http://lhscampaign.cf/Account/ResetPass?token=";
                var token = userManager.GetPasswordResetToken(user.Id);
                mail.Body += HttpUtility.UrlEncode(token) + "&userId=" + user.Id;
                mail.Body += " to reset your LHS|Campaign password.";

                smtpServer.Port = int.Parse(settings["SMTP Port"]);
                smtpServer.Credentials = new System.Net.NetworkCredential(settings["SMTP User"], settings["SMTP Pass"]);

                smtpServer.Send(mail);
                return Ok("sent");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("API/Account/ResetPass")]
        public IHttpActionResult ResetPassword(ResetPassModel model)
        {
            using (var userManager = new UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                // Thanks! http://stackoverflow.com/questions/19539579/how-to-implement-a-tokenprovider-in-asp-net-identity-1-1-nightly-build
                if (Startup.DataProtectionProvider != null)
                {
                    userManager.PasswordResetTokens = new DataProtectorTokenProvider(Startup.DataProtectionProvider.Create("PasswordReset"));
                    userManager.UserConfirmationTokens = new DataProtectorTokenProvider(Startup.DataProtectionProvider.Create("ConfirmUser"));
                }

                var result = userManager.ResetPassword(model.userId, model.token, model.password);
                if (result.Succeeded)
                {
                    return Ok("set");
                }
            }
            return Ok("problem");
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
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return Ok("no user");
            }

            if (!user.IsCandidate)
            {
                return Ok("set");
            }
            user.Candidate.Position = model.position;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetReasons")]
        public IHttpActionResult SetReasons(SetReasonsModel model)
        {
            var userId = User.Identity.GetUserId();

            var user = db.Users.Find(userId);
            if (user == null)
            {
                return Ok("no user");
            }

            var candidate = user.Candidate;
            if (candidate == null)
            {
                return Ok("not candidate");
            }

            candidate.Reasons = model.reasons;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetSocial")]
        public IHttpActionResult SetSocial(SetSocialModel model)
        {
            model.facebook = string.IsNullOrWhiteSpace(model.facebook) ? null : model.facebook.Trim();

            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return Ok("no user");
            }

            var candidate = user.Candidate;
            if (candidate == null)
            {
                return Ok("no candidate");
            }

            candidate.Facebook = model.facebook;
            db.SaveChanges();
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetPass")]
        public IHttpActionResult SetPass(SetPassModel model)
        {
            using (var userManager = new UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                var result = userManager.ChangePassword(User.Identity.GetUserId(), model.currPass, model.newPass);
                return Ok(result.Succeeded ? "set" : "nope");
            }
        }

        [HttpPost]
        [Route("API/Account/Register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            using (var userManager = new UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(db)))
            {
                var errors = new List<string>();

                // TODO: Should be validating with ModelState
                if (model.Password.Length <= 6) errors.Add("Password");
                if (!(model.Year <= 2018 && model.Year >= 2016)) errors.Add("Year");
                if (model.Position == null || model.Position.Length > 50) errors.Add("Position");
                model.Position = model.Position.ToLower();
                if (model.FullName == null || model.FullName.Length > 50) errors.Add("FullName");
                if (db.Users.Count(usr => usr.UserName == model.Username) > 0) errors.Add("Username");

                if (errors.Count > 0)
                {
                    return Ok(string.Join(",", errors) + ",");
                }

                var user = new User { UserName = model.Username, Email = model.Email, Year = model.Year };
                var preConf = db.PreConfs.FirstOrDefault(conf => conf.Email == model.Email.ToLower());
                if(preConf != null)
                {
                    user.IsConfirmed = true;
                    db.PreConfs.Remove(preConf);
                }
                if (!string.IsNullOrWhiteSpace(model.Position))
                {
                    if (string.IsNullOrWhiteSpace(model.FullName))
                    {
                        model.FullName = model.Username;
                    }

                    // create candidate for user
                    user.Candidate = new Candidate
                    {
                        Owner = user,
                        Position = model.Position,
                        Name = model.FullName
                    };
                }

                var result = await userManager.CreateAsync(user, model.Password);
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
