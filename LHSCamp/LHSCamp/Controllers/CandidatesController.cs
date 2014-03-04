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
    public class CandidatesController : ApiController
    {
        private LCDB db = new LCDB();

        // GET: api/Candidates/5
        [Route("API/Candidates/{position}")]
        public IHttpActionResult GetCandidates(string Position)
        {
            var candidates = db.Candidates.Where(c => c.Position.ToLower() == Position.ToLower());
            return Ok(candidates.Select(cand => new
            {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        [HttpPost]
        [Authorize]
        [Route("API/ChosenCandidates/Add")]
        public IHttpActionResult AddChosenCandidate(AddCandidateModel model)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == model.id);
            if (candidate == null) return NotFound();

            if (!currUser.ChosenCandidates.Contains(candidate))
            {
                currUser.ChosenCandidates.Add(candidate);
                db.SaveChanges();
            }
            return Ok(currUser.ChosenCandidates.Select(cand => new {
                id = cand.Id,
                name = cand.Name,
                position = cand.Position,
                profilePic = cand.ProfilePic
            }));
        }

        [HttpPost]
        [Authorize]
        [Route("API/ChosenCandidates/Remove")]
        public IHttpActionResult AddChosenCandidate(AddCandidateModel model)
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            var currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            if (currUser == null) return NotFound();

            var candidate = db.Candidates.FirstOrDefault(cand => cand.Id == model.id);
            if (candidate == null) return NotFound();

            if (currUser.ChosenCandidates.Contains(candidate))
            {
                currUser.ChosenCandidates.Remove(candidate);
                db.SaveChanges();
            }
            return Ok(currUser.ChosenCandidates.Select(cand => new
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