﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using LatinoNetOnline.Backend.Shared.Abstractions.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Data
{
    class SeedData
    {
        public static void EnsureSeedData(string connectionString, SettingOptions settingOptions)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<EventDbContext>(options =>
               options.UseNpgsql(connectionString));


            using var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            if (settingOptions.RunMigration)
            {

                var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();
                context.Database.Migrate();

            }

            if (settingOptions.Seed)
            {




            }

        }
    }
}
