
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

using WebPushDemo.Models;

namespace LatinoNetOnline.Backend.Modules.Notifications.Api.Data
{
    class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
    {
        public NotificationDbContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Bootstrapper/LatinoNetOnline.Backend.Bootstrapper"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
            var connectionString = config.GetConnectionString("Default");
            optionsBuilder.UseNpgsql(connectionString);
            return new NotificationDbContext(optionsBuilder.Options);
        }
    }
}
