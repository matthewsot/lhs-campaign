﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace LHSCamp.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public virtual Candidate Candidate { get; set; }
        public bool IsCandidate 
        {
            get { return Candidate != null; }
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Candidate
    {
        public virtual User Owner { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ProfilePic { get; set; }
    }

    public class LCDB : IdentityDbContext<User>
    {
        public DbSet<Candidate> Candidates { get; set; }
        public LCDB()
            : base("LCDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOptional(a => a.Candidate).WithRequired(a => a.Owner);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options)
        {
            var manager = new ApplicationUserManager(new UserStore<User>(new LCDB()));
            // Configure the application user manager
            manager.UserValidator = new UserValidator<User>(manager)
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