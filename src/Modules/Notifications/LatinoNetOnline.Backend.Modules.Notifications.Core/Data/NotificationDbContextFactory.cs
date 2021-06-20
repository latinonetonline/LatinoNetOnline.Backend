
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace LatinoNetOnline.Backend.Modules.Notifications.Core.Data
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
