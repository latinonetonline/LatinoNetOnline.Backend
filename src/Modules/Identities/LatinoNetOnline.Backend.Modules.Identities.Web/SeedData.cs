// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;

using IdentityServerHost.Models;

using LatinoNetOnline.Backend.Modules.Identities.Web.Data;
using LatinoNetOnline.Backend.Shared.Abstractions.Options;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Security.Claims;

namespace LatinoNetOnline.Backend.Modules.Identities.Web
{
    class SeedData
    {
        public static void EnsureSeedData(string connectionString, SettingOptions settingOptions, Options.IdentityOptions identityOptions)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(connectionString, o => o.MigrationsAssembly(typeof(Config).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            if (settingOptions.RunMigration)
            {

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

            }

            if (settingOptions.Seed)
            {

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                var roleAdminExist = roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult();
                var roleUserExist = roleManager.RoleExistsAsync("User").GetAwaiter().GetResult();

                if (!roleAdminExist)
                {
                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "Admin"
                    }).GetAwaiter().GetResult();
                }


                if (!roleUserExist)
                {
                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "User"
                    }).GetAwaiter().GetResult();
                }

                var superAdminUser = userMgr.FindByNameAsync("latinonetonline").GetAwaiter().GetResult();
                if (superAdminUser is null)
                {
                    superAdminUser = new ApplicationUser
                    {
                        Name = "Latino .NET",
                        Lastname = "Online",
                        UserName = "latinonetonline",
                        Email = "latinonetonline@outlook.com",
                        EmailConfirmed = true
                    };

                    var result = userMgr.CreateAsync(superAdminUser, identityOptions.Password).GetAwaiter().GetResult();

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    userMgr.AddToRoleAsync(superAdminUser, "Admin").GetAwaiter().GetResult();

                    userMgr.AddClaimsAsync(superAdminUser, new Claim[]{
                            new Claim(JwtClaimTypes.Subject, superAdminUser.Id),
                            new Claim(JwtClaimTypes.GivenName, superAdminUser.Name),
                            new Claim(JwtClaimTypes.FamilyName, superAdminUser.Lastname),
                            new Claim(JwtClaimTypes.Email, superAdminUser.Email)
                        }).GetAwaiter().GetResult();

                }
            }

        }
    }
}
