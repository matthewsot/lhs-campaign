using System.Collections.Generic;

namespace LHSCampaign.Models
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