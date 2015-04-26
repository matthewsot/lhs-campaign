using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LHSCamp.Models
{
    public class CandidateViewModel
    {
        public int Id { get; set; }

        public int GraduationYear { get; set; }
        public string Name { get; set; }

        public string ProfilePicture { get; set; }
        public string CoverPhoto { get; set; }
        public string Position { get; set; }

        public string Platform { get; set; }
        public ICollection<ExternalLink> ExternalLinks { get; set; }
    }
}