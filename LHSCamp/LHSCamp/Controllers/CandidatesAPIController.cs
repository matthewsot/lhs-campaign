using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LHSCamp.Models;

namespace LHSCamp.Controllers
{
    [Authorize]
    public class CandidatesAPIController : ApiController
    {
        private LCDB db = new LCDB();

        [AllowAnonymous]
        [Route("API/Anon/Candidates/{Position}")]
        public IHttpActionResult GetAnonCandidates(string Position)
        {
            var candidates = db.Candidates.Where(c => c.Position.ToLower() == Position.ToLower() && c.ProfilePic != null);
            return Ok(candidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        // GET: api/Candidates/5
        [Route("API/Candidates/{Position}")]
        public IHttpActionResult GetCandidates(string Position)
        {
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);
            //Thanks! http://stackoverflow.com/questions/654906/linq-to-entities-random-order
            var candidates = db.Candidates.Where(c => c.Position.ToLower() == Position.ToLower() && c.ProfilePic != null).OrderBy(b => Guid.NewGuid());

            return Ok(candidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                chosen = chosenCandidateIds.Contains(cand.Id)
            }).ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("API/Candidate/Details/{Id}")]
        public IHttpActionResult GetCandidateDetails(int Id)
        {
            var candidate = db.Candidates.Where(cand => cand.Id == Id).Select(cand => new
            {
                reasons = cand.Reasons,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                facebook = cand.Facebook
            }).FirstOrDefault();

            if (candidate == null)
                return NotFound();

            return Ok(candidate);
        }

        [HttpGet]
        [Route("API/Chosen")]
        public IHttpActionResult GetChosenCandidates()
        {
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);

            return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                chosen = chosenCandidateIds.Contains(cand.Id)
            }));
        }

        [HttpGet]
        [Route("API/Chosen/Add/{Id}")]
        public IHttpActionResult AddChosenCandidate(int Id)
        {
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == Id);
            if (candidate == null) return NotFound();

            if (!currUser.ChosenCandidates.Contains(candidate))
            {
                currUser.ChosenCandidates.Add(candidate);
                db.SaveChanges();
            }

            return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic,
                chosen = chosenCandidateIds.Contains(cand.Id)
            }).ToList());
        }

        [HttpGet]
        [Route("API/Chosen/Remove/{Id}")]
        public IHttpActionResult RemoveChosenCandidate(int Id)
        {
            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var chosenCandidateIds = currUser.ChosenCandidates.Select(cand => cand.Id);

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == Id);
            if (candidate == null) return NotFound();

            if (currUser.ChosenCandidates.Contains(candidate))
            {
                currUser.ChosenCandidates.Remove(candidate);
                db.SaveChanges();
            }
            return Ok("removed");
            //return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel()
            //{
            //    id = cand.Id,
            //    name = cand.Name,
            //    position = cand.Position,
            //    profilePic = cand.ProfilePic,
            //    chosen = chosenCandidateIds.Contains(cand.Id)
            //}));
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