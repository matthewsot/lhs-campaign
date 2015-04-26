using System.Collections.Generic;

namespace LHSCamp.Models
{
    public class CandidatesViewModel
    {
        public int GraduationYear { get; set; }
        public IEnumerable<PositionViewModel> Positions { get; set; }

        public CandidatesViewModel()
        {
            Positions = new List<PositionViewModel>();
        }
    }
}