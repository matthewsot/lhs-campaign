using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? @class)
        {
            var cookie = Request.Cookies["selected-class"];
            if (cookie != null && @class == null)
            {
                @class = int.Parse(cookie.Value);
            }
            else if (@class == null)
            {
                @class = 2017;
            }

            @class = (int)Math.Floor((decimal)@class);
            if (@class < 2016 || @class > 2018)
            {
                @class = 2017;
            }
            ViewBag.Year = @class;
            Response.Cookies.Set(new HttpCookie("selected-class", @class.ToString()));

            using (var db = new LCDB())
            {
                var positions = db.Users
                                    .Where(user => user.IsConfirmed && user.Candidate != null && user.Year == @class)
                                    .Select(user => user.Candidate)
                                    .GroupBy(cand => cand.Position.ToLower())
                                    .ToDictionary(c => c.Key, c => c.ToList());

                var finalPositions = new Dictionary<string, List<Candidate>>();

                foreach (var position in positions)
                {
                    if (
                        !(new[] {"secretary", "treasurer", "vice president", "president"}.Contains(
                            position.Key)))
                    {
                        continue;
                    }
                    var rand = new Random();
                    var candidates = position.Value.Where(candidate => candidate.ProfilePic != null).OrderBy(a => rand.Next()).ToList();
                    foreach (var cand in candidates)
                    {
                        cand.ToString();
                    }
                    finalPositions.Add(position.Key, candidates);
                }

                return View(finalPositions);
            }
        }
    }
}