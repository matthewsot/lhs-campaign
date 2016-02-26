using System.Collections.Generic;

namespace LHSCampaign.Models
{
    public class CandidateViewModel
    {
        public string Id { get; set; }

        public int GraduationYear { get; set; }
        public string Name { get; set; }

        public string ProfilePicture { get; set; }
        public string CoverPhoto { get; set; }
        public string Position { get; set; }

        public string Platform { get; set; }
        public ICollection<ExternalLink> ExternalLinks { get; set; }

        public bool IsConfirmed { get; set; }
        public string Email { get; set; }
        
        public CandidateViewModel(Candidate candidate)
        {
            Id = candidate.Id;
            GraduationYear = candidate.GraduationYear;
            Name = candidate.Name;
            ProfilePicture = candidate.ProfilePicture;
            CoverPhoto = candidate.CoverPhoto;
            Position = candidate.Position;
            Platform = candidate.Platform;
            ExternalLinks = candidate.ExternalLinks;
            IsConfirmed = candidate.IsConfirmed;
            Email = candidate.Email;
        }
    }
}