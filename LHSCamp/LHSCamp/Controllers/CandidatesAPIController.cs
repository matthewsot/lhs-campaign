using System;
using System.Linq;
using System.Web.Http;
using LHSCamp.Models;

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
            var candidates = db.Candidates.Where(c => c.Position.Equals(position, StringComparison.CurrentCultureIgnoreCase) && c.ProfilePic != null);
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
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);
            //Thanks! http://stackoverflow.com/questions/654906/linq-to-entities-random-order
            var candidates = db.Candidates.Where(c => c.Owner.Year == currUser.Year && c.Owner.IsConfirmed
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
                return NotFound();

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
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);

            return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel
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
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == id);
            if (candidate == null) return NotFound();

            if (currUser.ChosenCandidates.Contains(candidate)) return Ok("added");

            currUser.ChosenCandidates.Add(candidate);
            db.SaveChanges();
            return Ok("added");
        }

        [HttpGet]
        [Route("API/Chosen/Remove/{id}")]
        public IHttpActionResult RemoveChosenCandidate(int id)
        {
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var candidate = db.Candidates.Find(id);
            if (candidate == null) return NotFound();

            if (!currUser.ChosenCandidates.Contains(candidate)) return Ok("removed");
            currUser.ChosenCandidates.Remove(candidate);
            db.SaveChanges();
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

        private bool CandidateExists(int id)
        {
            return db.Candidates.Count(e => e.Id == id) > 0;
        }
    }
}