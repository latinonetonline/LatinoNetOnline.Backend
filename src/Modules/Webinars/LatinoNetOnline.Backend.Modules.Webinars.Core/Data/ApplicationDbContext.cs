
using LatinoNetOnline.Backend.Modules.Webinars.Core.Dto.Emails;
using LatinoNetOnline.Backend.Modules.Webinars.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System;

namespace LatinoNetOnline.Backend.Modules.Webinars.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Speaker> Speakers => Set<Speaker>();
        public DbSet<Proposal> Proposals => Set<Proposal>();
        public DbSet<UnavailableDate> UnavailableDates => Set<UnavailableDate>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cfs");

            modelBuilder.Entity<Speaker>(builder =>
            {
                builder.ToTable("Speakers");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Name).IsRequired();

                builder.Property(e => e.LastName).IsRequired();
                builder.Property(e => e.Email)
                .IsRequired();


                builder.Property(e => e.Description).IsRequired();

                builder.Property(e => e.Image)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired();

                builder.Property(p => p.UserId).IsRequired().HasDefaultValue(Guid.Empty);

               

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

                builder.Property(e => e.Flyer)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired(false);

                builder.Property(e => e.Streamyard)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired(false);

                builder.Property(e => e.Meetup)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired(false);

                builder.Property(e => e.LiveStreaming)
                .HasConversion(v => v.ToString(), db => new Uri(db))
                .IsRequired(false);

            });

            modelBuilder.Entity<UnavailableDate>(builder =>
            {
                builder.ToTable("UnavailableDates");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Reason).IsRequired();
                builder.Property(e => e.Date).IsRequired();
  

            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
