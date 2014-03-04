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