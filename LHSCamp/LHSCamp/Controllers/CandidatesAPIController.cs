using System;
using System.Linq;
using System.Web.Http;
using LHSCamp.Models;
using Microsoft.AspNet.Identity;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class CandidatesAPIController : ApiController
    {
        private LCDB db = new LCDB();

        [AllowAnonymous]
        [Route("API/Anon/Candidates/{position}")]
        public IHttpActionResult GetAnonCandidates(string position)
        {
            var candidates = db.Users.Where(c =>
                c.Position.Equals(position, StringComparison.CurrentCultureIgnoreCase)
                && c.ProfilePicture != null);

            return Ok(candidates.Select(cand => new CandidateModel
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        // GET: api/Candidates/5
        [Route("API/Candidates/{position}")]
        public IHttpActionResult GetCandidates(string position)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return NotFound();
            }

            var chosenCandidateIds = user.ChosenCandidates.Select(cand => cand.Id);

            // Thanks! http://stackoverflow.com/questions/654906/linq-to-entities-random-order
            var candidates = db.Candidates.Where(c => c.Owner.Year == user.GraduationYear && c.Owner.IsConfirmed
                && c.Position.ToLower() == position.ToLower() && c.ProfilePic != null)
                .OrderBy(b => Guid.NewGuid());

            return Ok(candidates.Select(cand => new CandidateModel
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                chosen = chosenCandidateIds.Contains(cand.Id)
            }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("API/Candidate/Details/{id}")]
        public IHttpActionResult GetCandidateDetails(int id)
        {
            var candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                reasons = candidate.Reasons,
                name = candidate.Name,
                position = candidate.Position,
                profilePic = candidate.ProfilePic,
                facebook = candidate.Facebook,
                email = candidate.Owner.Email,
                coverPhoto = candidate.CoverPhoto
            });
        }

        [HttpGet]
        [Route("API/Chosen")]
        public IHttpActionResult GetChosenCandidates()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user == null)
            {
                return NotFound();
            }

            var chosenCandidateIds = user.ChosenCandidates.Select(cand => cand.Id);

            return Ok(user.ChosenCandidates.Select(cand => new CandidateModel
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                chosen = chosenCandidateIds.Contains(cand.Id),
                coverPhoto = cand.CoverPhoto
            }));
        }

        [HttpGet]
        [Route("API/Chosen/Add/{id}")]
        public IHttpActionResult AddChosenCandidate(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == id);
            if (user == null || candidate == null)
            {
                return NotFound();
            }

            if (!user.ChosenCandidates.Contains(candidate))
            {
                user.ChosenCandidates.Add(candidate);
                db.SaveChanges();
            }

            return Ok("added");
        }

        [HttpGet]
        [Route("API/Chosen/Remove/{id}")]
        public IHttpActionResult RemoveChosenCandidate(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var candidate = db.Candidates.Find(id);
            if (user == null || candidate == null)
            {
                return NotFound();
            }

            if (user.ChosenCandidates.Contains(candidate))
            {
                user.ChosenCandidates.Remove(candidate);
                db.SaveChanges();
            }

            return Ok("removed");
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