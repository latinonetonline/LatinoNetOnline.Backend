
using LatinoNetOnline.Backend.Modules.Notifications.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Data
{
    class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices => Set<Device>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("notifications");

            builder.Entity<Device>(builder =>
            {
                builder.HasKey(e => e.Id);

                builder.Property(e => e.PushAuth).IsRequired();
                builder.Property(e => e.PushEndpoint).IsRequired();
                builder.Property(e => e.PushP256DH).IsRequired();
            });


            base.OnModelCreating(builder);
        }
    }
}
