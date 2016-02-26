using System.Collections.Generic;

namespace LHSCampaign.Models
{
    public class CandidatesViewModel
    {
        public int GraduationYear { get; set; }
        public ICollection<PositionViewModel> Positions { get; set; }
    }
}