using LatinoNetOnline.Backend.Bootstrapper;
using LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build()
    .InitRegisterModules()
    .RunAsync();