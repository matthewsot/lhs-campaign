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
    public class AddCandidateModel
    {
        public int id { get; set; }
    }
    public class CandidateModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string profilePic { get; set; }
    }
    public class CandidatesController : ApiController
    {
        private LCDB db = new LCDB();

        // GET: api/Candidates/5
        [Route("API/Candidates/{Position}")]
        public IHttpActionResult GetCandidates(string Position)
        {
            var candidates = db.Candidates.Where(c => c.Position.ToLower() == Position.ToLower());
            return Ok(candidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("API/Chosen")]
        public IHttpActionResult GetChosenCandidates()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("API/Chosen/Add/{Id}")]
        public IHttpActionResult AddChosenCandidate(int Id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

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
                profilePic = cand.ProfilePic
            }));
        }

        [HttpGet]
        [Authorize]
        [Route("API/Chosen/Remove/{Id}")]
        public IHttpActionResult RemoveChosenCandidate(int Id)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == Id);
            if (candidate == null) return NotFound();

            if (currUser.ChosenCandidates.Contains(candidate))
            {
                currUser.ChosenCandidates.Remove(candidate);
                db.SaveChanges();
            }
            return Ok(currUser.ChosenCandidates.Select(cand => new CandidateModel()
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
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