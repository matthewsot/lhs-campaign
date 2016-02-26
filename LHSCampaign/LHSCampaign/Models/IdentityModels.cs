using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LHSCampaign.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Candidate : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Candidate> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public int GraduationYear { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public string ProfilePicture { get; set; }
        public string CoverPhoto { get; set; }

        public string Platform { get; set; }
        public ICollection<ExternalLink> ExternalLinks { get; set; }
        public int ViewCount { get; set; }
    }


    public class ExternalLink
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Link { get; set; }
    }

    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class LogEntry
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Entry { get; set; }
    }

    public class PreConfirmation
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class LCDb : IdentityDbContext<Candidate>
    {
        public LCDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static LCDb Create()
        {
            return new LCDb();
        }
    }
}