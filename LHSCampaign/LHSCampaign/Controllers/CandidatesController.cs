using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHSCampaign.Models;

namespace LHSCampaign.Controllers
{
    public class CandidatesController : Controller
    {
        public LCDb db = new LCDb();

        [Route("Candidates/Class/{graduationYear}")]
        public ActionResult GetClass(int? graduationYear)
        {
            var model = new CandidatesViewModel();

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

            var rand = new Random();
            foreach (var position in positions)
            {
                if (!(new[] { "secretary", "treasurer", "vice president", "president" }.Contains(
                    position.Key)))
                {
                    continue;
                }

                var positionModel = new PositionViewModel { Name = position.Key };

                var candidates = position.Value
                    .Where(candidate => candidate.ProfilePicture != null)
                    .OrderBy(a => rand.Next())
                    .ToList();

                foreach (var cand in candidates)
                {
                    cand.ToString();
                }
                
                positionModel.Candidates = candidates.Select(cand => new CandidateViewModel(cand));

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

            var model = new CandidateViewModel(candidate);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}