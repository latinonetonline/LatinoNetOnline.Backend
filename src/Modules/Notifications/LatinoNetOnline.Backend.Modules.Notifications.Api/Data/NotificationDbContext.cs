using Microsoft.EntityFrameworkCore;

namespace WebPushDemo.Models
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebPushDemo.Models.Device> Devices { get; set; }

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
