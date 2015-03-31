using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LHSCamp.Models;

namespace LHSCamp.Controllers
{
    public class CandidatesController : Controller
    {
        // GET: Candidates
        public ActionResult Index()
        {
            return View();
        }

        [Route("Candidates/{id}")]
        public ActionResult Get(int id)
        {
            using (LCDB db = new LCDB())
            {
                var candidate = db.Candidates.Find(id);
                candidate.ToString();
                candidate.Owner.Year.ToString();
                return View(candidate);
            }
        }
    }
}