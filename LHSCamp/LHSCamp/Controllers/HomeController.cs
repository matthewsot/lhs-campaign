using System;
using System.Collections.Generic;
using System.Linq;
using LHSCamp.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace LHSCamp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int @class = 2017)
        {
            ViewBag.Year = @class;

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