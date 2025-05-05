using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSCI3110TermProject.Data
{
    /// <summary>
    /// My application’s EF Core DbContext,  
    /// inheriting Identity support for ApplicationUser.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSet for all job applications in the system
        public DbSet<JobApplication> JobApplications { get; set; }
        // DbSet for all tags (e.g., "Interviewed", "Offer") that I can attach to applications
        public DbSet<Tag> Tags { get; set; }
        // Join table between JobApplication and Tag (many-to-many)
        public DbSet<JobApplicationTag> JobApplicationTags { get; set; }

        /// <summary>
        /// Configure entity mappings, keys, and relationships.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Let Identity configure its own tables first
            base.OnModelCreating(builder);

            // Configure many-to-many for JobApplication ↔ Tag 
            // Define composite primary key on the join entity
            builder.Entity<JobApplicationTag>()
                .HasKey(jt => new { jt.JobApplicationId, jt.TagId });
            // One JobApplication can have many JobApplicationTags
            builder.Entity<JobApplicationTag>()
                .HasOne(jt => jt.JobApplication)
                .WithMany(j => j.JobApplicationTags)
                .HasForeignKey(jt => jt.JobApplicationId);
            // One Tag can be linked to many JobApplicationTags
            builder.Entity<JobApplicationTag>()
                .HasOne(jt => jt.Tag)
                .WithMany(t => t.JobApplicationTags)
                .HasForeignKey(jt => jt.TagId);
        }
    }
}
