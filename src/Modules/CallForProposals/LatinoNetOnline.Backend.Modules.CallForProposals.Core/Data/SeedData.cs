// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Options;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data
{
    class SeedData
    {
        public static void EnsureSeedData(string connectionString, SettingOptions settingOptions)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(connectionString));


            using var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            if (settingOptions.RunMigration)
            {

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

            }

            if (settingOptions.Seed)
            {




            }

        }
    }
}
