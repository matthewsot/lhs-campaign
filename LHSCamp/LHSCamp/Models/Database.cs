using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LHSCamp.Models
{
    // You can add profile data for the user by adding more properties to your Candidate class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Candidate : IdentityUser
    {
        public int GraduationYear { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        
        public string ProfilePicture { get; set; }
        public string CoverPhoto { get; set; }

        public string Platform { get; set; }
        public ICollection<ExternalLink> ExternalLinks { get; set; }
        public int ViewCount { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Candidate> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
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

    public class LCDB : IdentityDbContext<Candidate>
    {
        public DbSet<Setting> Config { get; set; }
        public DbSet<LogEntry> Log { get; set; }
        public DbSet<PreConfirmation> PreConfs { get; set; }

        public LCDB()
            : base("LCDB")
        {
        }
    }

    public class ApplicationUserManager : UserManager<Candidate>
    {
        public ApplicationUserManager(IUserStore<Candidate> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options)
        {
            var manager = new ApplicationUserManager(new UserStore<Candidate>(new LCDB()));
            // Configure the application user manager
            manager.UserValidator = new UserValidator<Candidate>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            manager.PasswordValidator = new MinimumLengthValidator(6);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.PasswordResetTokens = new DataProtectorTokenProvider(dataProtectionProvider.Create("PasswordReset"));
                manager.UserConfirmationTokens = new DataProtectorTokenProvider(dataProtectionProvider.Create("ConfirmUser"));
            }
            return manager;
        }
    }
}