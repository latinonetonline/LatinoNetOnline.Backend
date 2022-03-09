
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Dto.Emails;
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

        public DbSet<Speaker> Speakers => Set<Speaker>();
        public DbSet<Proposal> Proposals => Set<Proposal>();
        public DbSet<UnavailableDate> UnavailableDates => Set<UnavailableDate>();
        public DbSet<Webinar> Webinars => Set<Webinar>();

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
                .HasConversion(v => v.ToString(), db => new Email(db))
                .IsRequired();


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

            modelBuilder.Entity<UnavailableDate>(builder =>
            {
                builder.ToTable("UnavailableDates");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Reason).IsRequired();
                builder.Property(e => e.Date).IsRequired();
  

            });

            modelBuilder.Entity<Webinar>(builder =>
            {
                builder.ToTable("Webinars");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.LiveStreaming)
                    .HasConversion(v => v == null ? null : v.ToString(), db => db == null ? null : new Uri(db));

                builder.Property(e => e.Streamyard)
                    .HasConversion(v => v == null ? null : v.ToString(), db => db == null ? null : new Uri(db));

                builder.Property(e => e.MeetupId);

                builder.Property(e => e.Status).IsRequired();

                builder.Property(e => e.Flyer)
                    .HasConversion(v => v == null ? null : v.ToString(), db => db == null ? null : new Uri(db));
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
