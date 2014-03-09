using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Owin;

namespace LHSCamp.Controllers
{
    public class AccountAPIController : ApiController
    {
        public AccountAPIController()
        {
        }
        [HttpPost]
        [Route("API/Account/CheckName")]
        public IHttpActionResult CheckName(UserNameModel model)
        {
            using(LCDB db = new LCDB())
            {
                bool exists = (db.Users.Count(u => u.UserName == model.Username) > 0);
                if (exists)
                    return Ok("exists");
                else
                    return Ok("new");
            }
        }

        [HttpPost]
        [Route("API/Account/SetEmail")]
        [Authorize]
        public IHttpActionResult SetEmail(SetEmailModel model)
        {
            using (LCDB db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                if (user == null)
                    return NotFound();

                user.Email = model.email;
                db.SaveChanges();
            }
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetReasons")]
        [Authorize]
        public IHttpActionResult SetReasons(SetReasonsModel model)
        {
            using (LCDB db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                if (user == null)
                    return NotFound();

                var candidate = user.Candidate;
                if (candidate == null)
                    return Unauthorized();

                candidate.Reasons = model.reasons;
                db.SaveChanges();
            }
            return Ok("set");
        }

        [HttpPost]
        [Route("API/Account/SetSocial")]
        [Authorize]
        public IHttpActionResult SetSocial(SetSocialModel model)
        {
            using (LCDB db = new LCDB())
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                if (user == null)
                    return NotFound();

                var candidate = user.Candidate;
                if (candidate == null)
                    return Unauthorized();

                candidate.Facebook = model.facebook;
                db.SaveChanges();
            }
            return Ok("set");
        }
        
        [HttpPost]
        [Route("API/Account/Register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            using (var UserManager = new Microsoft.AspNet.Identity.UserManager<User>(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<User>(
                        new LCDB())))
            {
                var Errors = new List<string>();

                //TODO: Should be validating with ModelState
                if (model.Password.Length <= 6) Errors.Add("Password");
                if (model.Year != 2016 && model.Year != 2017) Errors.Add("Year");
                if (model.Position != null && model.Position.Length > 50) Errors.Add("Position");
                if (model.FullName != null && model.FullName.Length > 50) Errors.Add("FullName");
                using (LCDB db = new LCDB())
                {
                    if (db.Users.Count(usr => usr.UserName == model.Username) > 0) Errors.Add("Username");
                }

                if(Errors.Count > 0)
                    return Ok(string.Join(",", Errors) + ",");

                var user = new User() { UserName = model.Username, Email = model.Email };
                if(!string.IsNullOrWhiteSpace(model.Position))
                {
                    if(string.IsNullOrWhiteSpace(model.FullName))
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
    }
}
