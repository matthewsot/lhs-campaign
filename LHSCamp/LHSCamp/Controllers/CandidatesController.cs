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
        [ResponseType(typeof(Candidate))]
        public IHttpActionResult GetCandidate(int id)
        {
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return Ok(candidate);
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