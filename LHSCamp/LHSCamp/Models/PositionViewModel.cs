using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LHSCamp.Models
{
    public class PositionViewModel
    {
        public string Name { get; set; }
        public IEnumerable<CandidateViewModel> Candidates { get; set; }

        public PositionViewModel()
        {
            Candidates = new List<CandidateViewModel>();
        }
    }
}