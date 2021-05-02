using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using LatinoNetOnline.Backend.Bootstrapper;

using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build()
    .RunAsync();