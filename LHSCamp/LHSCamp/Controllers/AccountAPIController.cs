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
        public async Task<IHttpActionResult> CheckName(UserNameModel model)
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
                using (LCDB db = new LCDB())
                {
                    if (db.Users.Count(usr => usr.UserName == model.Username) > 0) Errors.Add("Username");
                }

                if(Errors.Count > 0)
                    return Ok(string.Join(",", Errors) + ",");

                var user = new User() { UserName = model.Username, Email = model.Email, Position = model.Position };
                user.IsCandidate = (!string.IsNullOrWhiteSpace(model.Position));

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
                    return Ok("NOIDEA");
                }
            }
        }
    }
}
