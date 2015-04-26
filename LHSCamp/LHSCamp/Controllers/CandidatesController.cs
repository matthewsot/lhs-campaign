using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using LHSCamp.Models;

namespace LHSCamp.Controllers
{
    public class CandidatesController : Controller
    {
        public LCDB db = new LCDB();

        [Route("Candidates/Class/{graduationYear}")]
        public ActionResult GetClass(int? graduationYear)
        {
            CandidatesViewModel model = new CandidatesViewModel();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Candidate", "Welcome");
            }
            
            graduationYear = graduationYear ??
                (Request.Cookies.AllKeys.Contains("selected-class") ?
                int.Parse(Request.Cookies["selected-class"].Value) : 2017);

            if (graduationYear < 2016 || graduationYear > 2018)
            {
                graduationYear = 2017;
            }

            model.GraduationYear = graduationYear.Value;
            Response.Cookies.Set(new HttpCookie("selected-class", graduationYear.ToString()));

            var positions = db.Users
                .Where(user => user.IsConfirmed && user.GraduationYear == graduationYear)
                .GroupBy(cand => cand.Position.ToLower())
                .ToDictionary(c => c.Key, c => c.ToList());

            foreach (var position in positions)
            {
                if (!(new[] {"secretary", "treasurer", "vice president", "president"}.Contains(
                    position.Key)))
                {
                    continue;
                }

                var positionModel = new PositionViewModel {Name = position.Key};

                var rand = new Random();
                var candidates = position.Value
                    .Where(candidate => candidate.ProfilePicture != null)
                    .OrderBy(a => rand.Next())
                    .ToList();

                foreach (var cand in candidates)
                {
                    cand.ToString();
                }

                positionModel.Candidates = Mapper.Map<List<Candidate>, List<CandidateViewModel>>(candidates);
                model.Positions.Add(positionModel);
            }

            return View();
        }

        [Route("Candidates/{id}")]
        public ActionResult GetCandidate(int id)
        {
            var candidate = db.Users.Find(id);

            candidate.ToString();

            candidate.ViewCount = candidate.ViewCount + 1;
            db.SaveChanges();

            var model = Mapper.Map<Candidate, CandidateViewModel>(candidate);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}