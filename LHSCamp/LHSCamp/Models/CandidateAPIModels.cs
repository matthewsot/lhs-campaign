using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LHSCamp.Models
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
        public bool chosen { get; set; }
    }
}