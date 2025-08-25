using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyPawDiaryApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePhotoPath { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostLike> Likes { get; set; }

        public virtual ICollection<Follow> Followers { get; set; } // People who follow me
        public virtual ICollection<Follow> Following { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
     

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // za multiple cascade paths problem
            modelBuilder.Entity<DailyActivity>()
                .HasRequired(a => a.Pet)
                .WithMany(p => p.DailyActivities)
                .HasForeignKey(a => a.PetId)
                .WillCascadeOnDelete(true); 

            modelBuilder.Entity<DailyActivityCompletion>()
                .HasRequired(c => c.Pet)
                .WithMany(p => p.DailyActivityCompletions)
                .HasForeignKey(c => c.PetId)
                .WillCascadeOnDelete(false);  

            modelBuilder.Entity<DailyActivityCompletion>()
                .HasRequired(c => c.Activity)
                .WithMany(a => a.Completions)
                .HasForeignKey(c => c.ActivityId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OneTimeActivity>()
                .HasRequired(a => a.Pet)
                .WithMany(p => p.OneTimeActivities)
                .HasForeignKey(a => a.PetId)
                .WillCascadeOnDelete(false);
                }



        public DbSet<Pet> Pets { get; set; }
        public DbSet<DailyActivity> DailyActivities { get; set; }

        public DbSet<OneTimeActivity> OneTimeActivities { get; set; }
        public DbSet<DailyActivityCompletion> DailyActivityCompletions { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
    }
}