﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHSCampaign.Models;

namespace LHSCampaign.Controllers
{
    public class CandidatesController : Controller
    {
        public LCDb db = new LCDb();

        [Route("Candidates")]
        public ActionResult Class(int? classOf)
        {
            //classOf = 0; //0 = ASB
            var model = new CandidatesViewModel();

            classOf = classOf ??
                (Request.Cookies.AllKeys.Contains("selected-class") ?
                int.Parse(Request.Cookies["selected-class"].Value) : 0);

            if (classOf < 0 || classOf > 1)
            {
                classOf = 0;
            }

            model.GraduationYear = classOf.Value;
            model.Positions = new List<PositionViewModel>();
            Response.Cookies.Set(new HttpCookie("selected-class", classOf.ToString()));

            var positions = db.Users
                .Where(user => user.IsConfirmed && user.GraduationYear == classOf)
                .GroupBy(cand => cand.Position.ToLower())
                .ToDictionary(c => c.Key, c => c.ToList());

            var rand = new Random();
            foreach (var position in positions)
            {
                if (!(new[] { "secretary", "treasurer", "vice president", "president" }.Contains(
                    position.Key.ToLower())))
                {
                    continue;
                }

                var positionModel = new PositionViewModel { Name = position.Key };

                var candidates = position.Value
                    //.Where(candidate => candidate.ProfilePicture != null)
                    .OrderBy(a => rand.Next())
                    .ToList();

                foreach (var cand in candidates)
                {
                    cand.ProfilePicture = cand.ProfilePicture ?? "";
                    cand.ToString();
                }
                
                positionModel.Candidates = candidates.Select(cand => new CandidateViewModel(cand));

                model.Positions.Add(positionModel);
            }

            return View(model);
        }

        [Route("Candidates/{id}")]
        public ActionResult GetCandidate(string id)
        {
            var candidate = db.Users.Find(id);

            if (candidate == null)
            {
                ViewBag.Id = id;
                return View("Removed");
            }

            candidate.ToString();

            candidate.ViewCount = candidate.ViewCount + 1;
            db.SaveChanges();

            var model = new CandidateViewModel(candidate);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}