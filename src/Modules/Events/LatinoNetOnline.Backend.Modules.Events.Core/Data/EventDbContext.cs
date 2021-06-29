using LatinoNetOnline.Backend.Modules.Events.Core.Entities;

using Microsoft.EntityFrameworkCore;

using System;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<Webinar> Webinars => Set<Webinar>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("events");

            modelBuilder.Entity<Webinar>(builder =>
            {
                builder.ToTable("Webinars");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.LiveStreaming)
                    .HasConversion(v => v == null ? null : v.ToString(), db => db == null ? null : new Uri(db));


                builder.Property(e => e.MeetupId);


                builder.Property(e => e.Flyer)
                    .HasConversion(v => v == null ? null : v.ToString(), db => db == null ? null : new Uri(db));
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
