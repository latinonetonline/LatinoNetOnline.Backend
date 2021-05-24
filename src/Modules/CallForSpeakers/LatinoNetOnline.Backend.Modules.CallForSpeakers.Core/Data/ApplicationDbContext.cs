
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Proposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cfs");

            modelBuilder.Entity<Speaker>(builder =>
            {
                builder.ToTable("Speakers");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Name).IsRequired();

                builder.Property(e => e.LastName).IsRequired();
                builder.Property(e => e.Email).IsRequired();
                builder.Property(e => e.Description).IsRequired();

                builder.Property(e => e.Image)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired();

            });

            modelBuilder.Entity<Proposal>(builder =>
            {
                builder.ToTable("Proposals");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Title).IsRequired();

                builder.Property(e => e.Description).IsRequired();
                builder.Property(e => e.EventDate).IsRequired();
                builder.Property(e => e.CreationTime).IsRequired();
                builder.Property(e => e.IsActive).IsRequired();

                builder.Property(e => e.AudienceAnswer);

                builder.Property(e => e.KnowledgeAnswer);
                builder.Property(e => e.UseCaseAnswer);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
